param (
  [Parameter(Position = 0, Mandatory = 1)] [ValidateSet('Release', 'Debug')] $Configuration,
  [Parameter(Position = 1, Mandatory = 0)] [switch] $GenerateDocs = $false,
  [Parameter(Position = 2, Mandatory = 0)] [string] $UserName,
  [Parameter(Position = 3, Mandatory = 0)] [string] $UserEmail
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

function Upload-Packages {
  Write-Host "TODO, deploy to Nuget."
}

function Build-Documentation {
  param(
    [Parameter(Position = 0, Mandatory = 1)] [System.IO.FileSystemInfo] $Directory
  )
  if (!(Get-Command -Name 'DocFx' -ErrorAction SilentlyContinue)) {
    choco install docfx -y
    if (!$?) { throw 'Could not install DocFx by chocolatey.' }
  }
  Push-Location $Directory
  & docfx docfx.json
  if (!$?) { throw 'Could not generate documentation.' }
  Pop-Location
}

function Generate-DocumentationPages {
  param(
    [Parameter(Position = 0, Mandatory = 1)] [System.IO.FileSystemInfo] $DocumentationDirectory,
    [Parameter(Position = 1, Mandatory = 1)] [System.IO.FileSystemInfo] $SrcDirectory
  )

  function Configure-Git {
    if ($env:GITHUB_ACCESS_TOKEN) {
      git config --global credential.helper store
      if (!$?) { throw "Could not set git command 'config --global credential.helper store'." }
      Add-Content "$env:USERPROFILE\.git-credentials" "https://$($env:GITHUB_ACCESS_TOKEN):x-oauth-basic@github.com`n" -ErrorAction Stop
    } else { Write-Host 'Could not find an access token for github, continuing by assuming push rights exists anyway.' -ForegroundColor Yellow }

    $name = git config --global user.name
    if (!$?) {
      if ([string]::IsNullOrWhiteSpace($UserEmail)) { throw "Invalid email parameter." }
      git config --global user.name $UserName
      if (!$?) { throw 'Could not set name for git config.' }
    } else { Write-Host "Found '$name' for 'git config --global user.name', skipping assignment." -ForegroundColor Yellow }

    $email = git config --global user.email
    if (!$?) {
      if ([string]::IsNullOrWhiteSpace($UserName)) { throw "Invalid parameter username parameter." }
      git config --global user.email $UserEmail
      if (!$?) { throw 'Could not set email for git config.' }
    } else { Write-Host "Found '$email' for 'git config --global user.email', skipping assignment." -ForegroundColor Yellow }
  }

  if (!$?) { throw "Could not obtain the SHA-1 for the current commit by running command 'git rev-parse HEAD'" }
  $appveyorBuildUri = "https://ci.appveyor.com/api/projects/$($env:APPVEYOR_ACCOUNT_NAME)/$($env:APPVEYOR_PROJECT_SLUG)/history?recordsNumber=2&branch=$($env:APPVEYOR_REPO_BRANCH)" 
  Write-Host "Requesting latest builds from uri '$appveyorBuildUri'."
  $previousBuildCommitId = Invoke-RestMethod -Uri $appveyorBuildUri -ErrorAction Stop `
    | Select-Object -ExpandProperty builds `
    | Select-Object -Last 1 -ExpandProperty commitId
    Write-Host "Comparing diffs with SHA-1 '$previousBuildCommitId'."
  # TODO SrcDirectory parameter should be a list for all directories the diff needs to be checked with.

  git diff --quiet --exit-code $previousBuildCommitId $SrcDirectory $DocumentationDirectory
  if ($LASTEXITCODE -eq 1) {
    Build-Documentation -Directory $DocumentationDirectory
    Configure-Git
    $ghPagesDirectory = 'gh_pages'
    git clone https://github.com/inputfalken/Lemonad.git -b gh-pages $ghPagesDirectory -q
    if (!$?) { throw "Could not clone 'gh-pages'." }
    $ghPagesDirectory = $ghPagesDirectory | Get-Item -ErrorAction Stop
    $docsSiteDirectory = Join-Path -Path $DocumentationDirectory -ChildPath '_site' -ErrorAction Stop | Get-Item -ErrorAction Stop
    $ghPagesDirectoryGitDirectory = Join-Path -Path $ghPagesDirectory -ChildPath '.git' -ErrorAction Stop | Get-Item -ErrorAction Stop -Force
    Copy-Item $ghPagesDirectoryGitDirectory $docsSiteDirectory -Recurse -ErrorAction Stop -Force
    Push-Location $docsSiteDirectory
    git add -A 2>&1
    if (!$?) { throw 'Failed adding generated documentation.' }
    git commit -m "Documentation updated" -q
    if (!$?) { throw 'Failed commiting generatated documentation.' }
    git push origin gh-pages -q
    if (!$?) { throw 'Failed pushing generated documentation.' }
    Pop-Location
    Remove-Item $ghPagesDirectory -Force -Recurse -ErrorAction Stop
    Remove-Item $docsSiteDirectory -Force -Recurse -ErrorAction Stop
  } elseif ($LASTEXITCODE -eq 0) { Write-Host 'No changes found when generating documentation for gh-pages' -ForegroundColor Yellow }
  else { throw 'Unhandled exit code for command ''git diff --exit-code''' }
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

# Automatic OS boolean environmental variables:
# * $isWindows
# * $IsMacOS
# * $IsLinux


if ($isWindows) {
  Pack-Projects -Directory $srcDiretory -Configuration $Configuration
  if ($env:APPVEYOR -and $env:CI) {
    switch ($env:APPVEYOR_REPO_BRANCH) {
      'master' {
        if (!$env:APPVEYOR_PULL_REQUEST_TITLE -and $GenerateDocs) {
          $documentationDirectory = (Join-Path -Path $rootDirectory -ChildPath 'docs' -ErrorAction Stop ) | Get-Item -ErrorAction Stop
          Generate-DocumentationPages -DocumentationDirectory $documentationDirectory -SrcDirectory $srcDiretory
          Upload-Packages
        }
      }
      [string]::Empty {
        throw 'Branch can not be empty'
      }
      $null {
        throw 'Branch can not be null'
      }
      default {
        exit
      }
    }
  }
}
