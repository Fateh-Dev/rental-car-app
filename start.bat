@echo off
title Parc Auto - Launcher
color 0A

echo ============================================
echo    PARC AUTO - Starting Backend + Frontend
echo ============================================
echo.

:: Start the .NET Backend in a new window
echo [1/2] Starting Backend (dotnet run) ...
start "Parc Auto - Backend" cmd /k "cd /d "%~dp0Backend" && dotnet run"

:: Give the backend a moment to initialize
timeout /t 5 /nobreak >nul

:: Start the Angular Frontend in a new window
echo [2/2] Starting Frontend (ng serve) ...
start "Parc Auto - Frontend" cmd /k "cd /d "%~dp0Frontend" && npm start"

echo.
echo ============================================
echo    Both servers are starting!
echo.
echo    Backend  : https://localhost:5001 (or http://localhost:5000)
echo    Frontend : http://localhost:4200
echo.
echo    Close this window anytime - servers
echo    will keep running in their own windows.
echo ============================================
echo.
pause
