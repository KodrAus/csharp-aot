# Publishing

```shell
$ dotnet publish -r win-x64 -c Release
```

will produce a `.dll` containing CIL alongside a binary that bootstraps the runtime. The CIL will be compiled into native code on the end-user's machine by the CoreCLR JIT.

> TODO: Instructions for using checked CoreCLR build + variables for dumping JIT'ed code at runtime.
