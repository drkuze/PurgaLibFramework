﻿Write-Host "=== PurgaLib DLL Installer ===" -ForegroundColor Cyan

$scpPath = Read-Host "Enter the path to the SCP:SL server folder"

if (!(Test-Path $scpPath)) {
    Write-Host "Invalid path" -ForegroundColor Red
    exit
}

$pluginPath = Join-Path $scpPath "LabAPI\Plugins\global"
$purgaDll = Join-Path $PSScriptRoot "PurgaLibFramework.dll"

if (!(Test-Path $purgaDll)) {
    Write-Host "PurgaLibFramework.dll not found in the same folder as this script" -ForegroundColor Red
    exit
}

New-Item -ItemType Directory -Force -Path $pluginPath | Out-Null
Copy-Item $purgaDll -Destination $pluginPath -Force

Write-Host "DLL copied to $pluginPath" -ForegroundColor Green
Write-Host "Installation completed!" -ForegroundColor Cyan