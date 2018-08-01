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
Test-Projects -Directory $testDirectory

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
            if (($pipeline | Where-Object {$_.IsRelease -eq $true}).count -gt 0 -and $GenerateDocs) {
              Generate-Documentation -DocumentationDirectory $documentationDirectory -Directories @($srcDiretory) -UserName $UserName -UserEmail $UserEmail 
            }

            $pipeline `
              | Format-Table `
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
