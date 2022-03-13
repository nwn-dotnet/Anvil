# Anvil

[![Downloads](https://img.shields.io/nuget/dt/NWN.Anvil)](https://www.nuget.org/packages/NWN.Anvil) [![License](https://img.shields.io/github/license/nwn-dotnet/Anvil)](https://github.com/nwn-dotnet/Anvil/blob/development/LICENSE) [![Release](https://img.shields.io/nuget/v/NWN.Anvil?label=Release)](https://github.com/nwn-dotnet/Anvil/releases) [![Development](https://img.shields.io/nuget/vpre/NWN.Anvil?label=Development)](https://www.nuget.org/packages/NWN.Anvil/#versions-tab) [![CI](https://github.com/nwn-dotnet/Anvil/actions/workflows/ci.yml/badge.svg)](https://github.com/nwn-dotnet/Anvil/actions/workflows/ci.yml)

Anvil is a C# framework for building behaviours and adding new functionalty to Neverwinter Nights: Enhanced Edition. The library allows server owners and builders to create simple behaviours, while giving plugin developers the option to implement complex systems or modify existing hardcoded rules.

Builders can add functionality like opening a store from a dialogue with a few lines of code, while plugin developers can leverage [NuGet](https://www.nuget.org/packages) to add new functionality with external packages and the [NWN.Native](https://github.com/nwn-dotnet/NWN.Native) library to completely rewrite existing game systems and mechanics.

- Latest [Release](https://github.com/nwn-dotnet/Anvil/releases/latest)
- [Changelog](https://github.com/nwn-dotnet/Anvil/blob/main/CHANGELOG.md) ([Development](https://github.com/nwn-dotnet/Anvil/blob/development/CHANGELOG.md))
- View the [API Reference](https://nwn-dotnet.github.io/Anvil/annotated.html)
- View [Community Submitted Plugins](https://github.com/nwn-dotnet/Anvil/discussions/categories/plugins)
- Join the community: [![Discord](https://img.shields.io/discord/714927668826472600?color=7289DA&label=Discord&logo=discord&logoColor=7289DA)](https://discord.gg/gKt495UBgS)

## Getting Started

New to Anvil and want to know how to get a server running? Have a look through our install guides: https://github.com/nwn-dotnet/Anvil/wiki/Installing-Anvil

Want to write some scripts in C# or develop a new plugin? Have a look through our plugin development guides: https://github.com/nwn-dotnet/Anvil/wiki/Plugin-Development

## Compiling Anvil
Anvil is a [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) project, and you will need the appropriate SDKs to compile and build the project.

`git` must also be available from the command line for build versioning.

## Tests
The repository includes a test runner project called `NWN.Anvil.TestRunner`, that is based on [NUnit](https://github.com/nunit/nunit).

This test runner executes a number of runtime tests that are declared in the `NWN.Anvil.Tests` project.

Both of these projects are Anvil plugins that can be installed on any server.

To run tests locally, install the plugins in the anvil home directory as documented [HERE](https://github.com/nwn-dotnet/Anvil/wiki/Installing-Plugins:-Local).

## Contributions
All contributions are welcome!

Join the discussion on [Discord](https://discord.gg/gKt495UBgS), and also see our [contribution guidelines](https://github.com/nwn-dotnet/Anvil/blob/development/CONTRIBUTING.md). 

## Credits
The Anvil Framework builds heavily on the foundations of the [NWNX:EE DotNET plugin](https://github.com/nwnxee/unified/tree/master/Plugins/DotNET) that was written by [Milos Tijanic](https://github.com/mtijanic "Milos Tijanic"), and derives several service implementations from plugins developed by the NWNX:EE team and its contributors.

## License
[MIT](https://github.com/nwn-dotnet/Anvil/blob/development/LICENSE)
