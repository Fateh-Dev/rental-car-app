# Parc Auto App Package Automation Script
$ErrorActionPreference = "Stop"

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "   Starting Build & Package Pipeline for Parc Auto" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan

# 1. Build Frontend
Write-Host "`n[1/5] Building Angular Frontend..." -ForegroundColor Yellow
Push-Location "Frontend"
try {
    npm run build
} finally {
    Pop-Location
}

# 2. Prepare Backend wwwroot
Write-Host "`n[2/5] Cleaning and Preparing Backend static folder..." -ForegroundColor Yellow
$wwwroot = Join-Path "Backend" "wwwroot"
if (Test-Path $wwwroot) {
    # Delete everything in wwwroot except for uploads folder to keep vehicle images
    Get-ChildItem $wwwroot | Where-Object { $_.Name -ne "uploads" } | Remove-Item -Recurse -Force
} else {
    New-Item -ItemType Directory -Path $wwwroot -Force > $null
}

# 3. Copy Frontend assets to Backend wwwroot
Write-Host "`n[3/5] Copying compiled Frontend assets to Backend..." -ForegroundColor Yellow
$frontendDist = Join-Path "Frontend" "dist\Frontend\browser"
if (-not (Test-Path $frontendDist)) {
    Write-Error "Angular build output was not found at $frontendDist"
}
Copy-Item -Path "$frontendDist\*" -Destination $wwwroot -Recurse -Force

# 4. Publish ASP.NET Core Backend
Write-Host "`n[4/5] Publishing unified ASP.NET Core Backend..." -ForegroundColor Yellow
$publishDir = Join-Path "Backend" "publish"
if (Test-Path $publishDir) {
    Remove-Item $publishDir -Recurse -Force
}
dotnet publish Backend/Backend.csproj -c Release -r win-x64 --self-contained true -o $publishDir

# 5. Compress published output to ZIP
Write-Host "`n[5/5] Compressing publish folder into ZIP..." -ForegroundColor Yellow
$zipPath = "parc-auto-app.zip"
if (Test-Path $zipPath) {
    Remove-Item $zipPath -Force
}
Compress-Archive -Path "$publishDir\*" -DestinationPath $zipPath -Force

Write-Host "`n==================================================" -ForegroundColor Green
Write-Host "   Application successfully packaged!" -ForegroundColor Green
Write-Host "   ZIP File: $zipPath" -ForegroundColor Green
Write-Host "==================================================" -ForegroundColor Green
