# Publishing

```powershell
dotnet publish -r win-x64 -c Release
```

will produce a `.dll` containing CIL and pre-compiled native code in the Ready To Run format alongside a binary that bootstraps the runtime.

You can use the `R2RDump.dll` tool (which requires a local build of [CoreCLR](https://github.com/dotnet/coreclr)) to inspect it:

```powershell
cd bin/Release/netcoreapp3.0/win-x64/publish

dotnet path/to/coreclr/bin/Product/Windows_NT.x64.Checked/R2RDump.dll --in CoreCLR.ReadyToRun.dll
```

To get actual disassembly, you need a library called `coredistools`. It's part of the [LLILC project](https://github.com/dotnet/llilc) and can be built from there and copied into the publish directory:

```powershell
cp path/to/llilc-llvm-clone/build/lib/coredistools.dll . 
```

(it'll be called `libcoredistools.dylib` on OSX or `libcoredistools.so` on Linux).

Then you can add a `-d` flag to `R2RDump.dll`:

```powershell
dotnet path/to/coreclr/bin/Product/Windows_NT.x64.Checked/R2RDump.dll --in CoreCLR.ReadyToRun.dll -d
```
