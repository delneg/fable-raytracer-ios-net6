# Fable Rust Raytracer - iOS version

Originally made by @ncave (https://github.com/ncave/fable-raytracer), port to iOS by @delneg



## Pre-requisites

* Rust, both stable and nightly
* arch64-apple-ios and x86_64-apple-ios toolchains
* .NET 6
* iOS workload for .NET (`sudo dotnet workload install ios `)

## How to launch

```bash
cd FableRustRaytracerNet6iOS/rust-src/
./build-rust.sh 
cd ..
dotnet run
```


## Notable points

* Zero-copy via `CGDataProvider(IntPtr memoryBlock, int size)` overload
* Creates animated images in parallel using F# Async
* Uses DLL import with Rust static lib
* Uses Rust from F# !