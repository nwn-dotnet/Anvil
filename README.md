# Anvil
Anvil is a C# framework for building behaviours and adding new functionalty to Neverwinter Nights: Enhanced Edition. The library allows server owners and builders to create simple behaviours, while giving plugin developers the option to implement complex systems or modify existing hardcoded rules.

Builders can add functionality like opening a store from a dialogue with a few lines of code, while plugin developers can leverage [NuGet](https://www.nuget.org/packages) to add new functionality with external packages and the [NWN.Native](https://github.com/nwn-dotnet/NWN.Native) library to completely rewrite existing game systems and mechanics.

- Latest [Release](https://github.com/nwn-dotnet/Anvil/releases/latest)
- [Changelog](https://github.com/nwn-dotnet/Anvil/blob/master/CHANGELOG.md) ([Development](https://github.com/nwn-dotnet/Anvil/blob/development/CHANGELOG.md))
- View [Community Submitted Plugins](https://github.com/nwn-dotnet/Anvil/discussions/categories/plugins)
- Join the community: [![Discord](https://img.shields.io/discord/714927668826472600?color=7289DA&label=Discord&logo=discord&logoColor=7289DA)](https://discord.gg/gKt495UBgS)

# Getting Started

### I use NWNX/NWScript code extensively. How hard is it to move to Anvil?

Anvil uses NWNX's DotNET plugin and is 100% compatible with existing NWNX/NWN servers.

If you are using docker, you can simply swap the NWN/NWNX image with Anvil's image.

All of your existing code will work of out the box, while offering the C# framework for you to begin developing plugins.

### Running Anvil - Docker
Anvil has its own docker images that are automatically configured to start and run Anvil and NWNXEE. Similar to the parent images, Anvil is configured by environment variables passed during `docker run`.

By default, Anvil will look in the `/nwn/anvil/Plugins` directory in the container for [managed plugins](#plugins--services). A volume containing your plugins can be mounted here to automatically run on startup.

Running the image is exactly the same as running the `beamdog/nwserver` or `nwnxee/unified` images. See the [NWNX](https://nwnxee.github.io/unified/) and [NWServer](https://hub.docker.com/r/beamdog/nwserver/) README's for configuring these images.

The following tags are supported:

|Tag|Example|
|-|-|
|`latest`|`nwndotnet/anvil:latest`|
|`<version>`|`nwndotnet/anvil:8193.22.1`|
|`<commit-hash>`|`nwndotnet/anvil:c09d2f6`|

### Running Anvil - Native
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
    |----NLog.dll
    |----NWN.Core.dll
    |----NWN.Anvil.deps.json
    |----NWN.Anvil.dll
    |----NWN.Anvil.runtimeconfig.dev.json
    |----NWN.Anvil.runtimeconfig.json
    |----NWN.Anvil.xml
    |----NWN.Native.dll
    |----LightInject.dll
    |----Plugins/
         |----YourPlugin/
              |----YourPlugin.dll
 ```
5. Configure NWNX options to the following:
```sh
NWNX_DOTNET_SKIP=n
NWNX_SWIG_DOTNET_SKIP=n
NWNX_DOTNET_ASSEMBLY=/your/path/to/NWN.Anvil # Where "NWN.Anvil.dll" was extracted in step 3, without the extension. E.g: NWNX_DOTNET_ASSEMBLY=/nwn/home/modbin/Anvil
# NWNX_DOTNET_ENTRYPOINT= # Make sure this option does not exist in your config
```
The DotNET and SWIG_DotNET plugins are required for the library to work. Make sure they are enabled!

For a step by step guide how to set up a local developement environment using your IDE of choice and windows. see [Development with Docker on Windows](Development_with_Docker_on_Windows.md).

### Configuration Options
The following options can be configured via environment variables:

|Variable|Default|Options|Description|
|-|-|-|-|
|`ANVIL_PLUGIN_PATH`|`/path/to/NWN.Anvil/Plugins`|A valid directory.|The root plugin path that Anvil will search for plugins.|
|`ANVIL_NLOG_CONFIG`||A valid path to a NLog XML config file.|See the [NLog Wiki](https://github.com/nlog/NLog/wiki/Configuration-file) for configuration options.|
|`ANVIL_RELOAD_ENABLED`|`false`|`true/false`|Enables support for plugin hot-reloading via `AnvilCore.Reload()`. Recommended for advanced users.|
|`ANVIL_PREVENT_START_NO_PLUGIN`|`false`|`true/false`|Prevents the server from starting if no plugins are detected/loaded.|
|`ANVIL_PRELINK_ENABLED`|`true`|`true/false`|Disables some initial startup checks when linking to the native NWN server binary. This is an advanced setting that should almost always be unset/set to true.
|`ANVIL_LOG_MODE`|`Off`|`Off/Duplicate/Redirect`|Configures redirection of the NWN server log. `Off` disables redirection, `Duplicate` creates a copy of the log entries in Anvil/NLog, `Redirect` redirects the log entries to Anvil/NLog, and skips the original log entry.|

# Builder/Developer's Guide

## Plugins & Services
Adding module behaviours starts by creating your own plugin assembly (.dll).

To get started, it is recommended to start by making a copy of the sample project found [HERE](https://github.com/nwn-dotnet/NWN.Samples/tree/master/managed/plugin-sample) with the package dependencies already setup for you.

The core of Anvil is built around a dependency injection model, and the library expects you to implement features in your plugins a similar way.

Using a class attribute (ServiceBinding), the system will automatically wire up all of the dependencies for that class as defined in the parameters of its constructor:

```csharp
using NLog;
using NWN.Services;

// The "ServiceBinding" attribute indicates this class will be created on start, and available to other classes as "ServiceA".
[ServiceBinding(typeof(ServiceA))]
public class ServiceA
{
  // Gets the logger for this service. By default, this reports to "logs.0/anvil.log"
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
Anvil is bundled with a core set of services that you can depend on in your own plugins and systems.

These services handle game events, task scheduling, and more! Examples of how to use these services can be found in the [Anvil docs](https://nwn-dotnet.github.io/Anvil/classAnvil_1_1Services_1_1ServiceBindingAttribute.html).

You can also find a full list of the services bundled by Anvil [HERE](https://nwn-dotnet.github.io/Anvil/namespaceAnvil_1_1Services.html).

## Credits
The Anvil Framework builds heavily on the foundations of the [NWNX:EE DotNET plugin](https://github.com/nwnxee/unified/tree/master/Plugins/DotNET) that was written by [Milos Tijanic](https://github.com/mtijanic "Milos Tijanic"), and derives several service implementations from plugins developed by the NWNX:EE team and its contributors.
