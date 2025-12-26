## Symptoms
- Server crashes when creating a HTTPS request (e.g. sending a webhook)
- Server crashes when calling a hashing function.
- The server crashes after the following Anvil startup log message:
```
Checking OpenSSL version. If the server crashes, see this page for troubleshooting
```
- You see the following warning in the anvil logs:
```
DotNET is currently using OpenSSL version 'xxx' which differs from the bundled game version 'yyy'.
```
- Server enters a segmentation fault loop, and you see the following log output repeating infinitely:
```
==============================================================
 NWNX Signal Handler:
==============================================================
 NWNX 8193.37-17 (73cf6ab) has crashed. Fatal error: Segmentation fault (11).
 Please file a bug at https://github.com/nwnxee/unified/issues
```

## Cause
The NWN server/client bundles an old version of the OpenSSL version library - 1.1.1.

Modern Linux distros no-longer include this version of OpenSSL, and instead include OpenSSL 3.

Since both versions are present when running the server, this causes a conflict when running DotNET as it will try to link to the system bundled OpenSSL 3 version, when OpenSSL 1 has already been initialized and referenced by the base game.

## Fix

To resolve the issue, you will need to manually compile OpenSSL and force .NET to use the older OpenSSL version.

### Compiling OpenSSL

1. Install build dependencies
```
apt-get install -y --no-install-recommends ca-certificates wget git build-essential
```

2. Download OpenSSL Sources and Compile

> If you want to put the libraries in a different location other than `/nwn/lib/`, change the `--prefix` and `--openssldir` values.

```
git clone https://github.com/openssl/openssl.git
cd openssl
git checkout OpenSSL_1_1_1t
./config --prefix=/nwn/lib/openssl --openssldir=/nwn/lib/openssl
make depend
make install
```

3. Confirm library files are generated

In the folder you specified for `--openssldir`, you should see the following folder structure:

![](~/images/troubleshooting/openssl/openssl1.png)

The `lib` folder should contain the following files:

![](~/images/troubleshooting/openssl/openssl2.png)

### Configuring Anvil/.NET

To force Anvil & .NET to use the correct OpenSSL libraries, you will need to configure the following environment variables.

These should be added to your server's start script:

```
# Path you specified for "--openssldir" + /lib
LD_LIBRARY_PATH="/nwn/lib/openssl/lib"

#Force .NET to use OpenSSL 1.1.1
CLR_OPENSSL_VERSION_OVERRIDE=1.1
DOTNET_OPENSSL_VERSION_OVERRIDE=1.1
```

If you are running a docker configuration, this should already be set for you. If you are not using the base Anvil docker images or have an exotic configuration, here are the variables you need to set:

```
# Path you specified for "--openssldir" + /lib
ENV NWN_LD_LIBRARY_PATH="/nwn/lib/openssl/lib"

#Force .NET to use OpenSSL 1.1.1
ENV CLR_OPENSSL_VERSION_OVERRIDE=1.1
ENV DOTNET_OPENSSL_VERSION_OVERRIDE=1.1
```
