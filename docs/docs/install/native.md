> [!WARNING]
> We recommend **Docker** over native installs as it isolates your server in a container. This can protect the rest of your system in the event that a malicious plugin is installed.

## Prerequisites
.NET 8.0: https://docs.microsoft.com/en-us/dotnet/core/install/linux

## Installing Anvil
1. Download and extract the dedicated server package: [Server packages](https://forums.beamdog.com/discussion/67157/server-download-packages-and-docker-support/p1)
2. Download and extract NWNX: https://github.com/nwnxee/unified/releases
3. Download and extract the Anvil [Release](https://github.com/nwn-dotnet/Anvil/releases) for your server/NWNX version.
4. The directory structure should look like the following:
```
    bin/
    |----linux-x86
         |----nwserver-linux
         |----NWNX_DotNET.so
         |----NWNX_SWIG_DotNET.so
    modbin/
    |----NWN.Anvil.deps.json
    |----NWN.Anvil.dll
    |----NWN.Anvil.runtimeconfig.dev.json
    |----NWN.Anvil.runtimeconfig.json
    |----NWN.Anvil.xml
    |----(Other dlls)
 ```
5. Configure NWNX options to the following:
```sh
NWNX_DOTNET_SKIP=n
NWNX_SWIG_DOTNET_SKIP=n
NWNX_DOTNET_ASSEMBLY=/your/path/to/NWN.Anvil # Where "NWN.Anvil.dll" was extracted in step 3, without the extension. E.g: NWNX_DOTNET_ASSEMBLY=/nwn/home/modbin/Anvil
NWNX_DOTNET_ENTRYPOINT=Anvil.AnvilCore
NWNX_DOTNET_METHOD=Bootstrap
NWNX_DOTNET_NEW_BOOTSTRAP=true
```
The DotNET and SWIG_DotNET plugins are required for the library to work. Make sure they are enabled!
