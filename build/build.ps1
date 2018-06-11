param (
  [Parameter(Position = 1, Mandatory = 1)]
  [ValidateSet('Release', 'Debug')]
  $Configuration
)

function Is-InsideGitRepository {
  if (Get-Command -Name 'git' -ErrorAction SilentlyContinue) {
    if (git rev-parse --is-inside-work-tree 2>$null) { $true } else { $false }
  } else {
    throw 'Git could not be found.'
  }
}

function Get-RootDirectory {
  if (Is-InsideGitRepository) {
    $location = git rev-parse --show-toplevel `
      | Resolve-Path `
      | Get-Item
    if (!$?) { throw 'could not move to git root directory.' }
    if ((Get-Location).Path -eq $location) { return Get-Location }
    else { return $location }
  } else { throw "'$(Get-Location)' is not a git directory/repository." }
}

function List-Files {
  [OutputType('System.IO.FileSystemInfo')]
  param()
  if (Is-InsideGitRepository) {
    $arguments = $args
    $input `
      | ForEach-Object `
      -Begin { $any = $false ; $joinedArgs = if ($arguments.Count -gt 0) { " $($arguments -join ' ')" } else { [string]::Empty } } `
      -Process { if (!$any) { $any = $true } ; "git ls-files $_" + $joinedArgs } `
      -End {
      if (!$any) {
        if ($joinedArgs -eq [string]::Empty) { "git ls-files *" }
        else { "git ls-files" + $joinedArgs }
      }
    } `
      | Invoke-Expression `
      | Get-Item
    if (!$?) { throw 'Could not list files with git.' }
  } else { throw "'$(Get-Location)' is not a git directory/repository." }
}

function Build-Solution {
  param(
    [Parameter(Position = 0, Mandatory = 1)] [System.IO.FileSystemInfo] $Solution,
    [Parameter(Position = 1, Mandatory = 1)] [string] $Configuration
  )
  dotnet build $solution --configuration $Configuration
  if (!$?) { throw "Could not build solution '$Solution'." }
}

function Test-Projects {
  param(
    [Parameter(Position = 0, Mandatory = 1)] [System.IO.FileSystemInfo] $Directory,
    [Parameter(Position = 1, Mandatory = 1)] [string] $Configuration
  )
  List-Files "$Directory*.csproj" `
    | ForEach-Object {
    dotnet test $_ --configuration $Configuration --no-build --no-restore
    if (!$?) { throw "Failed executing tests for project '$_'." }
  }
}

function Pack-Projects {
  param(
    [Parameter(Position = 0, Mandatory = 1)] [System.IO.FileSystemInfo] $Directory,
    [Parameter(Position = 1, Mandatory = 1)] [string] $Configuration
  )
  List-Files "$Directory*.csproj" `
    | ForEach-Object {
    dotnet pack $_ --configuration $Configuration --no-build --no-restore
    if (!$?) { throw "Failed creating NuGet package from project '$_'." }
  }
}

#-------------------------------------------------------------------------------------------------------------------------------------
$rootDirectory = Get-RootDirectory
$testDirectory = Join-Path -Path $rootDirectory -ChildPath 'test' `
  | Get-Item -ErrorAction Stop
$srcDiretory = Join-Path -Path $rootDirectory -ChildPath 'src' `
  | Get-Item -ErrorAction Stop
$buildDirectory = Join-Path -Path $rootDirectory -ChildPath 'build' `
  | Get-Item -ErrorAction Stop

Set-Location -Path $rootDirectory -ErrorAction Stop

$solution = Get-ChildItem -Filter '*.sln' `
  | ForEach-Object `
  -Begin {$result = $null} `
  -Process { if ($result) { throw "More than 1 solution was found in '$(Get-Location)'" } else { $result = $_ } } `
  -End { Get-Item $result }

Build-Solution -Solution $solution -Configuration $Configuration
Test-Projects -Directory $testDirectory -Configuration $Configuration

$isWindows = !($IsMacOS -or $IsLinux)

if ($isWindows) {
  Pack-Projects -Directory $srcDiretory -Configuration $Configuration
}
