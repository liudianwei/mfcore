set currentpath=%~dp0%
set parentPath=%cd%

cd %parentPath%\MF\

cd MF.ApiJob
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Cache
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Consul 
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Core 
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.FluentValidation
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Ioc
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Job
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Job.Core
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Localization.Json
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Log
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Mapster
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Masuit
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.MediatR
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Message
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Misdata
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.NetCoreApp
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Orm
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Rest
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Swagger
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Utils
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Utils.Excel
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Utils.SPC
dotnet pack --output ./../../core-nupkgs

cd ..
cd SimplCommerce.Modules
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Authorization
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.CodeGen
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.Mqtt
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.TTS
dotnet pack --output ./../../core-nupkgs

cd ..
cd MF.ClickHouse
dotnet pack --output ./../../core-nupkgs

pause