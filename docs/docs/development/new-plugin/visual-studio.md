## Prerequisites

This guide assumes you have completed the environment setup for Visual Studio, and you already have an Anvil server up and running.

If you have not yet installed Visual Studio, or the project templates, please walk through this tutorial first: https://github.com/nwn-dotnet/Anvil/wiki/Setting-up-your-development-environment

If you have not yet got a working server, please look at the [Getting started guides](https://github.com/nwn-dotnet/Anvil#getting-started) in the Readme of the Anvil repository.

## Creating a new project
1. Launch Visual Studio

<details>
<summary>2. In the startup screen, click "Continue without code"</summary>

![](https://i.imgur.com/IQavXPW.png)
</details>

<details>
<summary>3. Open the options menu</summary>

![](https://i.imgur.com/Td1B87G.png)
</details>

<details>
<summary>4. Ensure the "Show all .NET Core templates in the New project dialog" option is checked, under "Environment/Preview Features", then click OK.</summary>

![](https://i.imgur.com/jqLzbqb.png)
</details>

5. Restart Visual Studio

<details>
<summary>6. In the start screen, click "Create a new project"</summary>

![](https://i.imgur.com/IeIpVqF.png)
</details>

<details>
<summary>7. In the new project window, use the dropdown filters to find the "NWN DotNET Anvil Plugin" template, then click next.</summary>

![](https://i.imgur.com/K9uwcU9.png)
</details>

<details>
<summary>8. Select the parent directory and name for your plugin, then click create.</summary>

* In this example, we will call the plugin `MyFirstPlugin`.

![](https://i.imgur.com/3TvH8CY.png)
</details>

## Choosing a new Anvil version

<details>
<summary>9. In the "Tools" menu, select "NuGet Package Manager/Manage NuGet Packages for Solution"</summary>

![](https://i.imgur.com/tWJiwuX.png)
</details>

<details>
<summary>10. In the "Installed" tab, click "NWN.Anvil"</summary>

![](https://i.imgur.com/JZxaf3u.png)
</details>

<details>
<summary>11. Check the box next to your plugin project, then select the desired version using the dropdown and click "Install"</summary>

![](https://i.imgur.com/tlWSUo1.png)

![](https://i.imgur.com/4WApguG.png)
</details>

## Creating your first build

<details>
<summary>12. If you have not already, re-open your project by selecting the ".sln" file</summary>

![](https://i.imgur.com/qx2J20L.png)
</details>

<details>
<summary>13. In the "Build" menu, click "Build Solution"</summary>

![](https://i.imgur.com/pgzB0IP.png)

This will build the source files in your project, and produce a plugin binary.

You should see the following in the output window:

![](https://i.imgur.com/Ua0nbOF.png)
</details>

<details>
<summary>14. Click "Open Folder in File Explorer" to navigate to the project folder.</summary>

![](https://i.imgur.com/WHJ4imc.png)
</details>

<details>
<summary>15. Navigate to the "YourPluginName" folder in "bin/Debug"</summary>

![](https://i.imgur.com/Z1PnCHg.png)

When submitting plugins for review in the [plugins forum](https://github.com/nwn-dotnet/Anvil/discussions/categories/plugins), copying the plugin to a server, or sharing the plugin with other people, you will need to send the whole `bin/Debug/YourPluginName` folder for the plugin to work.
</details>

<details>
<summary>16. Copy the "YourPluginName" folder to the Anvil Plugin directory. This directory should be inside your NWN home directory (modules/hak/etc), under `anvil/plugins`</summary>

* E.g. if your plugin is called `MyFirstPlugin`, the directory structure should look like the following
```
    hak/
    tlk/
    modules/
    anvil/
    |----Plugins/
         |----MyFirstPlugin/
              |----MyFirstPlugin.deps.json
              |----MyFirstPlugin.dll
              |----MyFirstPlugin.pdb
```
</details>

<details>
<summary>17. Launch the server! If everything is done correctly, you should see the plugin loading as a message</summary>

```
I [2021/07/02 23:52:04.035] [Anvil.Internal.LoggerManager] Using Logger config: "/nwn/anvil/Plugins/FRC/nlog.config"
nwnxee-server_1  | I [2021/07/02 23:52:04.064] [Anvil.AnvilCore] Prelinking native methods.
nwnxee-server_1  | I [2021/07/02 23:52:04.199] [Anvil.AnvilCore] Prelinking complete.
nwnxee-server_1  | I [2021/07/02 23:52:04.199] [Anvil.AnvilCore] Loading NWN.Anvil 8193.23.0.0 (NWN.Core: 8193.23.4.0, NWN.Native: 8193.23.3.0).
nwnxee-server_1  | I [2021/07/02 23:52:04.199] [Anvil.AnvilCore] .NET runtime is ".NET 5.0.6", running on "Linux 5.4.72-microsoft-standard-WSL2 #1 SMP Wed Oct 28 23:40:43 UTC 2020", installed at "/usr/share/dotnet/shared/Microsoft.NETCore.App/5.0.6/"
nwnxee-server_1  | I [2021/07/02 23:52:04.199] [Anvil.AnvilCore] Server is running Neverwinter Nights 8193.23.
nwnxee-server_1  | I [2021/07/02 23:52:04.199] [NWN.Plugins.PluginLoader] Loading 1 DotNET plugin/s from: /nwn/anvil/Plugins
nwnxee-server_1  | I [2021/07/02 23:52:04.327] [NWN.Plugins.PluginLoader] Loading DotNET plugin (MyFirstPlugin) - /nwn/anvil/Plugins/MyFirstPlugin/MyFirstPlugin.dll
nwnxee-server_1  | I [2021/07/02 23:52:04.336] [NWN.Plugins.PluginLoader] Loaded DotNET plugin (MyFirstPlugin) - /nwn/anvil/Plugins/MyFirstPlugin/MyFirstPlugin.dll
```

</details>

## Next Steps

You should now have a working environment for developing plugins, and can start looking into using the Anvil API to write your very first plugin!

Usage of the Anvil API is covered in general tutorials, which can be found [HERE](https://github.com/nwn-dotnet/Anvil/wiki).
