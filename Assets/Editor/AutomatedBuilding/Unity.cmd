REM command for Jenkins: .\Assets\Editor\AutomatedBuilding\Unity.cmd

@echo off
REM This script is for Jenkins to call the build script inside Unity

setlocal enableextensions enabledelayedexpansion

title Unity Build Script

@echo on

set UNITY_PATH="C:\Program Files\Unity\Editor"
set PROJECT_PATH="D:\Jenkins\workspace\Akaneiro game client"

cd %UNITY_PATH%
Unity.exe -batchmode -nographics -projectPath %PROJECT_PATH% -executeMethod AutomatedBuilder.PerformBuild -quit

@echo off
REM Check to see if this was run at the command line or by double clicking file
echo %cmdcmdline% | findstr /l "\"\"" >NUL
if %ERRORLEVEL% EQU 0 (
	REM pause if run by double clicking file
	echo.
	pause
)
