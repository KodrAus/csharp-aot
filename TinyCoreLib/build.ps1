param (
    [string]$csc = "csc",
    [string]$corelibpath = "Test.CoreLib.dll",
    [string]$ilc = "ilc",
    [string]$clang = "clang++",
    [string]$nativelibsearchpath = ""
)

if (!(Test-Path -Path ./bin)) {
    New-Item -ItemType Directory ./bin
}

Write-Host "Running csc"

. $csc Program.cs /out:bin/TinyCoreLib.dll /noconfig /nostdlib /r:$corelibpath


Write-Host "Running ilc"

. $ilc bin/TinyCoreLib.dll -o bin/TinyCoreLib.o -r $corelibpath --systemmodule Test.CoreLib


Write-Host "Running clang"

. $clang bin/TinyCoreLib.o -o bin/TinyCoreLib -L $nativelibsearchpath -l bootstrapper -l Runtime -w
