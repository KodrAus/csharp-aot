# Startup time

This little sample demonstrates one common way to run .NET web apps: in some serverless environment where the app might not be running until a request comes in. In this scenario, startup time is really important.

This sample is made up of two parts:

- `App`: a typical web API that can be built for CoreCLR with JIT, CoreCLR with Ready To Run, or CoreRT.
- `Host`: a quick-and-dirty proxy that will spin up an instance of `App` and forward a request to it.

## Running the sample

```powershell
./build.ps1 -rid win-x64
./run.ps1 -rid win-x64
```

This will build the `App` in various modes and start up the proxy `Host`. You can then call:

```powershell
./call.ps1 -kind jit -path api/weather
```

To send a request that'll get forwarded. The `kind` parameter can be one of `jit`, `r2r` or `aot`. Use the logs from the running `Host` to observe the difference in request time between the various kinds.
