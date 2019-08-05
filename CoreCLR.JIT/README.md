# Publishing

```powershell
dotnet publish -r win-x64 -c Release
```

will produce a `.dll` containing CIL alongside a binary that bootstraps the runtime. The CIL will be compiled into native code on the end-user's machine by the [CoreCLR](https://github.com/dotnet/coreclr) JIT.

## Viewing CIL

You can peek at the CIL using a tool called `ildasm` from a local Checked build of [CoreCLR](https://github.com/dotnet/coreclr):

```powershell
path/to/coreclr/bin/Product/Windows_NT.x64.Checked/ildasm CoreCLR.JIT.dll
```

## Viewing JIT'ed code

Your local Checked build of [CoreCLR](https://github.com/dotnet/coreclr) can also dump the JIT'ed code at runtime:

```powershell
$env:COMPlus_JitDisasm="CoreCLR.JIT.Program:Main(ref)"
```

```powershell
cd bin/Release/netcoreapp3.0/win-x64/publish
cp path/to/coreclr/bin/Product/Windows_NT.x64.Checked/* .

./corerun CoreCLR.JIT.dll
```
