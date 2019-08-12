param (
    [string]$rid
)

rm -r App/out/jit
dotnet publish App/App.csproj -r $rid -c Release -o App/out/jit

rm -r App/out/r2r
dotnet publish App/App.csproj /p:ReadyToRun=true -r $rid -c Release -o App/out/r2r

rm -r App/out/aot
dotnet publish App/App.csproj /p:AOT=true -r $rid -c Release -o App/out/aot

dotnet publish Host/Host.csproj -c Release -r $rid
