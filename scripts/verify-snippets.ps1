<#
.SYNOPSIS
    Compile-checks every C# snippet against the installed ATLAS Automation API DLLs.

.DESCRIPTION
    Each snippet under snippets/csharp is a self-contained file with top-level
    statements. This script compiles each one in isolation against the ATLAS
    Automation API and Client assemblies, so a broken snippet fails CI before it
    ever reaches a user. It does NOT run the snippets (that needs a live ATLAS).

.PARAMETER AtlasDir
    Folder containing MAT.Atlas.Automation.Api.dll and MAT.Atlas.Automation.Client.dll.
    Defaults to the standard ATLAS 10/11 install location, or $env:ATLAS_DIR if set.

.EXAMPLE
    pwsh scripts/verify-snippets.ps1
#>
[CmdletBinding()]
param(
    [string]$AtlasDir = $(if ($env:ATLAS_DIR) { $env:ATLAS_DIR } else { 'C:\Program Files\McLaren Applied Technologies\ATLAS 10' })
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$snippetRoot = Join-Path $repoRoot 'snippets\csharp'

$apiDll = Join-Path $AtlasDir 'MAT.Atlas.Automation.Api.dll'
$clientDll = Join-Path $AtlasDir 'MAT.Atlas.Automation.Client.dll'

foreach ($dll in @($apiDll, $clientDll)) {
    if (-not (Test-Path $dll)) {
        Write-Error "Could not find $dll. Install ATLAS or pass -AtlasDir."
    }
}

# Scratch project that references the installed DLLs.
$work = Join-Path ([System.IO.Path]::GetTempPath()) "atlas-snippet-verify"
if (Test-Path $work) { Remove-Item $work -Recurse -Force }
New-Item -ItemType Directory -Path $work | Out-Null

@"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0-windows</TargetFramework>
    <Nullable>disable</Nullable>
    <ImplicitUsings>disable</ImplicitUsings>
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include="Program.cs" />
    <Reference Include="MAT.Atlas.Automation.Api"><HintPath>$apiDll</HintPath></Reference>
    <Reference Include="MAT.Atlas.Automation.Client"><HintPath>$clientDll</HintPath></Reference>
  </ItemGroup>
</Project>
"@ | Set-Content -Path (Join-Path $work 'verify.csproj') -Encoding utf8

$snippets = Get-ChildItem -Path $snippetRoot -Filter '*.cs' -Recurse | Sort-Object FullName
$failures = @()

foreach ($snippet in $snippets) {
    Copy-Item $snippet.FullName (Join-Path $work 'Program.cs') -Force
    $rel = $snippet.FullName.Substring($repoRoot.Length + 1)
    $output = & dotnet build (Join-Path $work 'verify.csproj') -c Release --nologo -v quiet 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  PASS  $rel" -ForegroundColor Green
    }
    else {
        Write-Host "  FAIL  $rel" -ForegroundColor Red
        $output | Where-Object { $_ -match 'error' } | ForEach-Object { Write-Host "        $_" -ForegroundColor DarkYellow }
        $failures += $rel
    }
}

Write-Host ""
if ($failures.Count -gt 0) {
    Write-Host "$($failures.Count) of $($snippets.Count) snippet(s) failed to compile." -ForegroundColor Red
    exit 1
}

Write-Host "All $($snippets.Count) snippets compiled successfully." -ForegroundColor Green
