@echo off
@cls

echo ===============================
echo  Limpiando carpeta build...
echo ===============================

set PROJECT_DIR=%~dp0..
set BUILD_DIR=%PROJECT_DIR%\build

if exist "%BUILD_DIR%" (
    echo Eliminando carpeta build...
    rmdir /S /Q "%BUILD_DIR%"
) else (
    echo Nada que limpiar. La carpeta build no existe.
)

echo ===============================
echo  Limpieza completada
echo ===============================
pause
