set currentpath=%~dp0%
set parentPath=%cd%

cd %parentPath%\MF\

cd BC.BasicData
nuget pack BasicData.csproj -OutputDirectory ./../../core-nupkgs

cd ..
cd BC.MacroScadaApp
nuget pack P2.MacroScadaApp.csproj -OutputDirectory ./../../core-nupkgs

cd ..
cd BC.MES.Common
nuget pack P0.MES.Common.csproj -OutputDirectory ./../../core-nupkgs

cd ..
cd BC.MIS.Monitor
nuget pack P3.MIS.Monitor.csproj -OutputDirectory ./../../core-nupkgs

pause