set currentpath=%~dp0%
set parentPath=%cd%

cd %parentPath%\MF\
cd MF.TTS
dotnet pack --output ./../../core-nupkgs
pause