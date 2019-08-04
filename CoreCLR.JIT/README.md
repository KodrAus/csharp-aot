# Publishing

```powershell
dotnet publish -r win-x64 -c Release
```

will produce a `.dll` containing CIL alongside a binary that bootstraps the runtime. The CIL will be compiled into native code on the end-user's machine by the CoreCLR JIT.

You can use a local Checked build of CoreCLR to dump the JIT'ed assembly at runtime:

```powershell
$env:COMPlus_JitPrintInlinedMethods=1
$env:COMPlus_JitDisasm="CoreCLR.JIT.Program:Main(ref)"
```

```powershell
cd bin/Release/netcoreapp3.0/win-x64/publish
cp path/to/coreclr/bin/Product/Windows_NT.x64.Checked/* .

./corerun CoreCLR.JIT.dll
```
