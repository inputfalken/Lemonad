param(
  [Parameter(Position = 0, Mandatory = 1)] [System.IO.FileSystemInfo] $DocumentationDirectory,
  [Parameter(Position = 1, Mandatory = 1)] [string] $UserName,
  [Parameter(Position = 2, Mandatory = 1)] [string] $UserEmail
)

function Install-Documentation {
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

Install-Documentation -Directory $DocumentationDirectory
$ghPagesDirectory = 'gh_pages'
# APPVEYOR_REPO_NAME - repository name in format owner-name/repo-name
git clone "https://github.com/inputfalken/lemonad" -b gh-pages $ghPagesDirectory -q
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
