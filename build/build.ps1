param (
  [Parameter(Position = 0, Mandatory = 1)] [ValidateSet('Release', 'Debug')] $Configuration,
  [Parameter(Position = 1, Mandatory = 0)] [switch] $GenerateDocs = $false,
  [Parameter(Position = 2, Mandatory = 0)] [string] $UserName,
  [Parameter(Position = 3, Mandatory = 0)] [string] $UserEmail
)

. (Join-Path $PSScriptRoot 'functions.ps1') -Configuration $Configuration -Source 'https://api.nuget.org/v3/index.json'

$rootDirectory = Get-RootDirectory
$testDirectory = Join-Path -Path $rootDirectory -ChildPath 'test' `
  | Get-Item -ErrorAction Stop
$srcDiretory = Join-Path -Path $rootDirectory -ChildPath 'src' `
  | Get-Item -ErrorAction Stop
$buildDirectory = Join-Path -Path $rootDirectory -ChildPath 'build' `
  | Get-Item -ErrorAction Stop
$documentationDirectory = (Join-Path -Path $rootDirectory -ChildPath 'docs' -ErrorAction Stop ) `
  | Get-Item -ErrorAction Stop
$solution = $rootDirectory `
  | Get-Solution

Build-Solution -Solution $solution

List-Files (Join-Path -Path $testDirectory -ChildPath '*.csproj') | ForEach-Object {
  dotnet test $_ --configuration $Configuration --no-build --no-restore
  if (!$?) { throw "Failed executing tests for project '$_'." }
}

if ($isWindows) {
  if ($env:APPVEYOR -and $env:CI) {
    switch ($env:APPVEYOR_REPO_BRANCH) {
      'master' {
        if (!$env:APPVEYOR_PULL_REQUEST_TITLE) {
          $pipeline = List-Files (Join-Path -Path $srcDiretory -ChildPath '*.csproj') `
            | ForEach-Object { Write-Host "Attempting to gather project info for '$_'." ; $_ } `
            | Get-ProjectInfo `
            | Pack-Package -ArtifactPath $rootDirectory -SourceCodePath $srcDiretory `
            | Upload-Package

          if ($pipeline | Test-Any { $_.IsRelease -eq $true }) {
            Write-Host 'New release found, Attempting to update documentation.' -ForegroundColor Yellow
            Generate-Documentation -DocumentationDirectory $documentationDirectory -Directories @($srcDiretory) -UserName $UserName -UserEmail $UserEmail 
          } else { Write-Host 'No new release found, skipping documentation generation.' -ForegroundColor Yellow  }

          Write-Host 'Printing build pipeline summary.' -ForegroundColor Yellow
          $pipeline `
            | Format-List `
            | Write-Output
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
