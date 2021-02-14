# NWN.Managed
NWN.Managed is a C# framework for building behaviours and adding new functionalty to Neverwinter Nights: Enhanced Edition. The library allows server owners and builders to create simple behaviours, while giving plugin developers the option to implement complex systems or modify existing hardcoded rules.

Builders can add functionality like opening a store from a dialogue with a few lines of code, while plugin developers can leverage [NuGet](https://www.nuget.org/packages) to add new functionality with external packages and the [NWN.Native](https://github.com/nwn-dotnet/NWN.Native) library to completely rewrite existing game systems and mechanics.

- Latest [Release](https://github.com/nwn-dotnet/NWN.Managed/releases/latest)
- View [Community Submitted Plugins](https://github.com/nwn-dotnet/NWN.Managed/discussions/categories/plugins)
- Join the community: [![Discord](https://img.shields.io/discord/714927668826472600?color=7289DA&label=Discord&logo=discord&logoColor=7289DA)](https://discord.gg/gKt495UBgS)

# Getting Started

### Running NWN.Managed - Docker
NWN.Managed has its own docker images that are automatically configured to start and run NWN.Managed and NWNXEE. Similar to the parent images, NWN.Managed is configured by environment variables passed during `docker run`.

By default, NWN.Managed will look in the `/nwn/nwnm/Plugins` directory in the container for [managed plugins](#plugins--services). A volume containing your plugins can be mounted here to automatically run on startup.

Running the image is exactly the same as running the `beamdog/nwserver` or `nwnxee/unified` images. See the [NWNX](https://nwnxee.github.io/unified/) and [NWServer](https://hub.docker.com/r/beamdog/nwserver/) README's for configuring these images.

The following tags are supported:

|Tag|Example|
|-|-|
|`latest`|`nwndotnet/nwn.managed:latest`|
|`<version>`|`nwndotnet/nwn.managed:8193.20.29`|
|`<commit-hash>`|`nwndotnet/nwn.managed:c09d2f6`|

### Running NWN.Managed - Native
1. Download and extract the dedicated server package: [Server packages](https://forums.beamdog.com/discussion/67157/server-download-packages-and-docker-support/p1)
2. Download and extract NWNX: https://github.com/nwnxee/unified/releases
3. Download and extract the NWN.Managed [Release](https://github.com/nwn-dotnet/NWN.Managed/releases) for your server/NWNX version.
4. The directory structure should look like the following:
```
    bin/
    |----linux-x86
         |----nwserver-linux
         |----NWNX_DotNET.so
         |----NWNX_Object.so
         |----NWNX_SWIG_DotNET.so
         |----NWNX_Util.so
    modbin/
    |----NLog.dll
    |----NWN.Core.dll
    |----NWN.Managed.deps.json
    |----NWN.Managed.dll
    |----NWN.Managed.runtimeconfig.dev.json
    |----NWN.Managed.runtimeconfig.json
    |----NWN.Managed.xml
    |----NWN.Native.dll
    |----SimpleInjector.dll
    |----Plugins/
         |----YourPlugin/
              |----YourPlugin.dll
 ```
5. Configure NWNX options to the following:
```sh
NWNX_DOTNET_SKIP=n
NWNX_OBJECT_SKIP=n
NWNX_UTIL_SKIP=n
NWNX_DOTNET_ASSEMBLY=/your/path/to/NWN.Managed # Where "NWN.Managed.dll" was extracted in step 3, without the extension. E.g: NWNX_DOTNET_ASSEMBLY=/nwn/home/modbin/NWN.Managed
# NWNX_DOTNET_ENTRYPOINT= # Make sure this option does not exist in your config
```
The DotNET, Object and Util plugins are required for the library to work. Make sure they are enabled!

Other plugins are optional, but may be required to access some extension APIs. An exception will be raised if you try to use an extension without the dependent plugin loaded.

For a step by step guide how to set up a local developement environment using your IDE of choice and windows. see [Development with Docker on Windows](Development_with_Docker_on_Windows.md).

### Configuration Options
The following options can be configured via environment variables:

|Variable|Default|Description|
|-|-|-|
|`NWM_PLUGIN_PATH`|`/path/to/NWN.Managed/Plugins`|The root plugin path that NWN.Managed will search for plugins.|
|`NWM_NLOG_CONFIG`||Custom path to a NLog XML config file. See the [NLog Wiki](https://github.com/nlog/NLog/wiki/Configuration-file) for configuration options.|
|`NWM_RELOAD_ENABLED`|`false`|Enables support for plugin hot-reloading via `NManager.Reload()`. Recommended for advanced users.|

# Builder/Developer's Guide

## Plugins & Services
Adding module behaviours starts by creating your own plugin assembly (.dll).

To get started, it is recommended to start by making a copy of the sample project found [HERE](https://github.com/nwn-dotnet/NWN.Samples/tree/master/managed/plugin-sample) with the package dependencies already setup for you.

The core of NWN.Managed is built around a dependency injection model, and the library expects you to implement features in your plugins a similar way.

Using a class attribute (ServiceBinding), the system will automatically wire up all of the dependencies for that class as defined in the parameters of its constructor:

```csharp
using NLog;
using NWN.Services;

// The "ServiceBinding" attribute indicates this class will be created on start, and available to other classes as "ServiceA".
[ServiceBinding(typeof(ServiceA))]
public class ServiceA
{
  // Gets the logger for this service. By default, this reports to "logs.0/nwm.log"
  private static readonly Logger Log = LogManager.GetCurrentClassLogger();

  // As "ServiceA" has the ServiceBinding attribute, this constructor will be called during server startup.
  public ServiceA()
  {
    Log.Info("Service A Loaded!");
  }
}

// This class will be available to other classes as "ServiceB".
[ServiceBinding(typeof(ServiceB))]
public class ServiceB
{
  private static readonly Logger Log = LogManager.GetCurrentClassLogger();

  // As "ServiceB" also has the ServiceBinding attribute, this constructor will also be called during server startup,
  // but since "ServiceA" is specified as a parameter (dependency), it will only be started after "ServiceA" has loaded.
  public ServiceB(ServiceA serviceA)
  {
    Log.Info("Service B Loaded!");
  }
}
```

Checking in the console or server logs, you should get the following output:
```
[ServiceA] Service A Loaded!
[ServiceB] Service B Loaded!
```

## Core Services
NWN.Managed is bundled with a core set of services that you can depend on in your own plugins and systems.

These services handle game events, task scheduling, and more! Examples of how to use these services can be found in the [NWN.Managed docs](https://nwn-dotnet.github.io/NWN.Managed/classNWN_1_1Services_1_1ServiceBindingAttribute.html#details).

You can also find a full list of the services bundled by NWN.Managed [HERE](https://nwn-dotnet.github.io/NWN.Managed/namespaceNWN_1_1Services.html).
