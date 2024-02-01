set currentpath=%~dp0%
set parentPath=%cd%

cd %parentPath%\MF\

cd MF.Cache
dotnet pack --output ./../../core-nupkgs


cd ..
cd MF.Orm
dotnet pack --output ./../../core-nupkgs


cd %parentPath%\MF.Modules\

cd MF.Modules.User.Shared
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Modules.UserCenter
dotnet pack --output ./../../core-nupkgs

pause