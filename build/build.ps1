param (
  [Parameter(Position = 0, Mandatory = 1)] [ValidateSet('Release', 'Debug')] $Configuration,
  [Parameter(Position = 1, Mandatory = 0)] [switch] $GenerateDocs = $false,
  [Parameter(Position = 2, Mandatory = 0)] [string] $UserName,
  [Parameter(Position = 3, Mandatory = 0)] [string] $UserEmail,
  [Parameter(Position = 4, Mandatory = 0)] [string] $Source = 'https://api.nuget.org/v3/index.json'
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
    git rev-parse --show-toplevel `
      | Resolve-Path `
      | Get-Item
    if (!$?) { throw 'could not move to git root directory.' }
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

function Pack-Package {
  param (
    [Parameter(Position = 0, Mandatory = 0, ValueFromPipeline)] $InputObject,
    [Parameter(Position = 1, Mandatory = 1)] [System.IO.FileSystemInfo] $ArtifactPath,
    [Parameter(Position = 2, Mandatory = 1)] [System.IO.FileSystemInfo] $SourceCodePath,
    [Parameter(Position = 3, Mandatory = 0)] [string] $ArtifactName = 'artifacts'
  )
  $ArtifactDirectory = Join-Path -Path $ArtifactPath -ChildPath $ArtifactName -ErrorAction Stop `
    | New-Item -Type Directory -Name $ArtifactName -Force -ErrorAction Stop

  if ($Input) { $InputObject = $Input }
  # For some reason this pipe spits out bad elements which is filtered out below.
  $InputObject `
    | ForEach-Object {
      if ($_.IsRelease) {
        dotnet pack $_.Path --configuration $Configuration --no-build --output $ArtifactDirectory | Out-Null
        if (!$?) { throw "Could not pack project" }
      }
      $_
    } `
    | Select-Object Path, Project, LocalVersion, OnlineVersion, IsRelease, @{Name = 'PackageOrNull'; Expression = {Join-Path -Path $ArtifactDirectory -ChildPath "$($_.Path.BaseName).$($_.LocalVersion).nupkg" | Get-Item -ErrorAction SilentlyContinue} }
}

function Upload-Package {
  $Input | ForEach-Object {
    # Check if path exists and if 'IsRelease' is true.
    if (($_.PackageOrNull -ne $null) -and [bool](Get-Item $_.PackageOrNull -ErrorAction SilentlyContinue) -and ($_.IsRelease)) {
      Write-Host "Pushing Package '$($_.PackageOrNull)' to '$Source'."
      dotnet nuget push $_.PackageOrNull --api-key $env:NUGET_API_KEY --source $Source | Out-Null
      if (!$?) { throw "Could not push package '$($_.PackageOrNull)' to NuGet (source : '$Source')." }
    }
    $_
  }
}

function Build-Documentation {
  param(
    [Parameter(Position = 0, Mandatory = 1)] [System.IO.FileSystemInfo] $Directory
  )
  if (!(Get-Command -Name 'DocFx' -ErrorAction SilentlyContinue)) {
    choco install docfx -y --version 2.37
    if (!$?) { throw 'Could not install DocFx by chocolatey.' }
  }
  Push-Location $Directory
  & docfx docfx.json
  if (!$?) { throw 'Could not generate documentation.' }
  Pop-Location
}

function Generate-Documentation {
  param([Parameter(Position = 0, Mandatory = 1)] [System.IO.FileSystemInfo] $DocumentationDirectory)

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

  $currentSha1 = git rev-parse HEAD
  if (!$?) { throw "Could not obtain the SHA-1 for the current commit by running command 'git rev-parse HEAD'" }
  $appveyorBuildUri = "https://ci.appveyor.com/api/projects/$($env:APPVEYOR_ACCOUNT_NAME)/$($env:APPVEYOR_PROJECT_SLUG)/history?recordsNumber=10&startBuildId=$($env:APPVEYOR_BUILD_ID)&branch=$($env:APPVEYOR_REPO_BRANCH)"
  Write-Host "Requesting previous builds from uri '$appveyorBuildUri'." -ForegroundColor Yellow
  $previousSha1 = Invoke-RestMethod -Uri $appveyorBuildUri -ErrorAction Stop `
    | Select-Object -ExpandProperty builds `
    | Select-Object buildNumber, commitId, pullRequestId `
    | Where-Object { $_.pullRequestId -eq $null } `
    | Sort-Object buildNumber -Descending `
    | Select-Object -ExpandProperty commitId `
    | Where-Object {$_ -ne $currentSha1} `
    | Select-Object -First 1
  Write-Host "Comparing diffs with '$currentSha1' '$previousSha1'." -ForegroundColor Yellow

  git diff --quiet --exit-code $previousSha1 $currentSha1 $DocumentationDirectory (($args | ForEach-Object { "'$_'" }) -join ' ')
  if ($LASTEXITCODE -eq 1) {
    Build-Documentation -Directory $DocumentationDirectory
    Configure-Git
    $ghPagesDirectory = 'gh_pages'
    # APPVEYOR_REPO_NAME - repository name in format owner-name/repo-name
    git clone "https://github.com/$($env:APPVEYOR_REPO_NAME)" -b gh-pages $ghPagesDirectory -q
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

function Get-ProjectInfo () {
  # Fetch the online version
  function Get-OnlineVersion ([string] $PackageName) {
    $packageName = NuGet list $PackageName -Source $Source -NonInteractive
    if (!$?) { throw "Something went wrong when retrieving package '$PackageName'." }
    # If no pacakge is found, it's assumed that it's the first release.
    if ($packageName -like 'No packages found.') { return [version] '0.0.0' }
    else { return [version] (($packageName | Select-Object -First 1).Split(" ") | Select-Object -Last 1) }
  }
  $input `
    | Select-Xml -XPath  '//AssemblyName|//Version' `
    | Sort-Object -Property Node `
    | Group-Object Path  `
    | Select-Object -Property `
  @{Name = 'Project'; Expression = { $_.Group[0].Node.InnerText} }, `
  @{Name = 'LocalVersion'; Expression = { [version]$_.Group[1].Node.InnerText} }, `
  @{Name = 'Path'; Expression = { $_.Name | Get-Item -ErrorAction Stop } } `
    | Select-Object -Property Project, LocalVersion , Path, @{Name = 'OnlineVersion'; Expression = { Get-OnlineVersion -PackageName  $_.Project} } `
    | Select-Object -Property Project, LocalVersion , Path, OnlineVersion, @{Name = 'IsRelease'; Expression = { $_.LocalVersion -gt $_.OnlineVersion } } `

}


#-------------------------------------------------------------------------------------------------------------------------------------


$rootDirectory = Get-RootDirectory
$testDirectory = Join-Path -Path $rootDirectory -ChildPath 'test' `
  | Get-Item -ErrorAction Stop
$srcDiretory = Join-Path -Path $rootDirectory -ChildPath 'src' `
  | Get-Item -ErrorAction Stop
$buildDirectory = Join-Path -Path $rootDirectory -ChildPath 'build' `
  | Get-Item -ErrorAction Stop

$documentationDirectory = (Join-Path -Path $rootDirectory -ChildPath 'docs' -ErrorAction Stop ) | Get-Item -ErrorAction Stop

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
  if ($env:APPVEYOR -and $env:CI) {
    switch ($env:APPVEYOR_REPO_BRANCH) {
      'master' {
        if (!$env:APPVEYOR_PULL_REQUEST_TITLE) {
          if ($GenerateDocs) { Generate-Documentation -DocumentationDirectory $documentationDirectory $srcDiretory }
          List-Files (Join-Path -Path $srcDiretory -ChildPath '*.csproj') `
            | Get-ProjectInfo `
            | Pack-Package -ArtifactPath $rootDirectory -SourceCodePath $srcDiretory `
            | Upload-Package
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
