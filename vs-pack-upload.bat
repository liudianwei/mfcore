set currentpath=%~dp0%
set parentPath=%cd%

cd %parentPath%\core-nupkgs\

dotnet nuget push -s http://192.168.0.111:9009/v3/index.json -k amescore "*.nupkg" --skip-duplicate

pause