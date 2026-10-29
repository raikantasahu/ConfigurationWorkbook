@echo off
setlocal EnableExtensions

rem Convenience wrapper for ConfigWorkbook.
rem Copy this file to run.bat and fill in the settings below. Git ignores run.bat,
rem so your settings and credentials stay out of the repository.
rem
rem Usage:  run.bat <itemtype> [extra ConfigWorkbook options]
rem   e.g.  run.bat Part
rem         run.bat Part -o "C:\out\Part Configuration Workbook.xlsx"
rem
rem Each setting below can be overridden by setting the matching environment
rem variable before calling, e.g.  set INNOVATOR_PASSWORD=... & run.bat Part

rem The Innovator server URL, e.g. http://<server>/<web-alias>
if not defined INNOVATOR_URL      set "INNOVATOR_URL="
rem The database name.
if not defined INNOVATOR_DB       set "INNOVATOR_DB="
rem The user. ApplySQL needs an administrator by default.
if not defined INNOVATOR_USER     set "INNOVATOR_USER="
rem The password, or its MD5 as produced by Innovator.ScalcMD5. IOM accepts the MD5
rem in place of the plaintext. Note it is still a credential: anyone holding this
rem value can log in as INNOVATOR_USER. Leave it empty to be prompted.
if not defined INNOVATOR_PASSWORD set "INNOVATOR_PASSWORD="

set "EXE=%~dp0bin\Debug\net10.0\ConfigWorkbook.exe"

if "%~1"=="" goto :usage

if not defined INNOVATOR_URL  goto :settings
if not defined INNOVATOR_DB   goto :settings
if not defined INNOVATOR_USER goto :settings

if not exist "%EXE%" (
    echo Building ConfigWorkbook...
    dotnet build "%~dp0ConfigWorkbook.csproj" -v q --nologo
    if errorlevel 1 exit /b 1
)

rem First argument is the ItemType name; anything after it is passed through.
set "ITEMTYPE=%~1"
shift

set "REST="
:collect
if "%~1"=="" goto :run
set "REST=%REST% %1"
shift
goto :collect

:run
rem Without -p, ConfigWorkbook prompts for the password.
set "PASSWORD_OPTION="
if defined INNOVATOR_PASSWORD set PASSWORD_OPTION= -p "%INNOVATOR_PASSWORD%"
"%EXE%" -u "%INNOVATOR_URL%" -d "%INNOVATOR_DB%" -n "%INNOVATOR_USER%"%PASSWORD_OPTION% -t "%ITEMTYPE%"%REST%
exit /b %ERRORLEVEL%

:settings
echo Set INNOVATOR_URL, INNOVATOR_DB and INNOVATOR_USER in %~nx0, or as environment variables.
exit /b 2

:usage
echo Usage: %~nx0 ^<itemtype^> [extra ConfigWorkbook options]
echo.
echo   Connects to %INNOVATOR_URL% [%INNOVATOR_DB%] as %INNOVATOR_USER%.
echo.
echo   %~nx0 Part                         write "Part Configuration Workbook.xlsx"
echo   %~nx0 Part -o C:\out\Part.xlsx     write to a given file
exit /b 2
