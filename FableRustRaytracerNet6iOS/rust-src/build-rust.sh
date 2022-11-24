#!/bin/bash

# Ensure that `cargo` is in PATH, using the default location.
. "$HOME/.cargo/env"

set -x

# Go to repo's root
# cd "${SRCROOT}/rust-src/" || exit

# Build binaries
#cargo +ios-arm64-1.57 build -Zbuild-std --target aarch64-apple-ios --release --lib
#cargo +nightly build -Zbuild-std --target aarch64-apple-ios --release --lib
cargo +nightly build -Zbuild-std --target aarch64-apple-ios-sim --release --lib
#cargo  build  --target=x86_64-apple-ios --release

#cargo +ios-arm64-1.57.0 build --target aarch64-apple-ios --release --lib
#cargo build --target=x86_64-apple-ios --release

strip ./target/aarch64-apple-ios-sim/release/libcore.a
#strip ./target/aarch64-apple-ios/release/libcore.a

# Create fat binary
#libtool -static -o ./core/libcore ./target/aarch64-apple-ios/release/libcore.a ./target/aarch64-apple-ios-sim/release/libcore.a

libtool -static -o ./core/libcore ./target/aarch64-apple-ios-sim/release/libcore.a

# Create C header
#cbindgen --config cbindgen.toml --crate rust-src --output core/bind_header.h
