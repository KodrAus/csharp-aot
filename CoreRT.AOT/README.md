# Publishing

```shell
dotnet publish -r win-x64 -c Release
```

will produce a binary containing native code, compiled with RyuJIT (the JIT used by [CoreCLR](https://github.com/dotnet/coreclr)) and linked up with the [CoreRT runtime](https://github.com/dotnet/corert) using the [object writer from LLILC](https://github.com/dotnet/llilc), which in turn is built on LLVM.

## Debugging

You can debug a .NET app compiled for CoreRT using LLDB:

```
cd bin/Release/netcoreapp3.0/win-x64/publish

lldb ./CoreRT.AOT
```

```
run
```

We can view the native stack:

```
bt
```

it includes both the native CoreRT frames and our C#, because it's compiled and linked together ahead-of-time using a native toolchain.

On Windows and Linux you should be able to see C# source inline (it looks like CoreRT might have trouble with Mach-O debug symbols).
