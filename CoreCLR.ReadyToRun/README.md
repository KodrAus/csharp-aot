# Publishing

```shell
$ dotnet publish -r win-x64 -c Release
```

will produce a `.dll` containing CIL and pre-compiled native code in the Ready To Run format alongside a binary that bootstraps the runtime. You can use the `R2RDump.dll` tool (which requires a local build of CoreCLR) to inspect it:

```shell
$ cd bin/Release/netcoreapp3.0/win-x64/publish
$ dotnet path/to/coreclr/bin/Product/Windows_NT.x64.Checked/R2RDump.dll --in CoreCLR.ReadyToRun.dll -d
```
