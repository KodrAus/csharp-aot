# C# Ahead-of-time

Some examples of compiling C# ahead-of-time.

## `CoreCLR.JIT`

Just-in-time (JIT) compilation. This is what you get when you install the `dotnet` SDK and call `dotnet build`.

## `CoreCLR.ReadyToRun`

Pre-JIT compilation. The JIT is run ahead-of-time so there's less work to do when first starting your app, but uses the same `CoreCLR` runtime.

## `CoreRT.AOT`

Full ahead-of-time compilation with a specially optimized runtime.
