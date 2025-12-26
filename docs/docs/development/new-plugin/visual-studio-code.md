## Prerequisites

This guide assumes you have completed the environment setup for Visual Studio Code, and you already have an Anvil server up and running.

If you have not yet installed Visual Studio Code, or the project templates, please walk through this tutorial first: https://github.com/nwn-dotnet/Anvil/wiki/Setting-up-your-development-environment

If you have not yet got a working server, please look at the [Getting started guides](https://github.com/nwn-dotnet/Anvil#getting-started) in the Readme of the Anvil repository.

## Creating a new project
1. Launch Visual Studio Code

<details>
<summary>2. Using the "File" dropdown, click "Open Folder"</summary>

![](https://i.imgur.com/U1CX26g.png)
</details>

<details>
<summary>3. Navigate to/create a new empty folder. This folder will store the files for your plugin project</summary>

- In this example, we will call the plugin `MyFirstPlugin`
- Click `Select Folder` when you are happy with the location.

![](https://i.imgur.com/eJhrlAw.png)
</details>

<details>
<summary>4. Using the "Terminal" dropdown, click "New Terminal"</summary>

![](https://i.imgur.com/Qyv3sNc.png)

At the bottom of the screen, you should see a new terminal that is pointing to the folder you selected in step 3.

![](https://i.imgur.com/spaEao7.png)
</details>

<details>
<summary>5. In the terminal, run the following commands</summary>

```
dotnet new --install NWN.Templates
dotnet new anvilplugin
```

You should see the following terminal output

![](https://i.imgur.com/P1xgqeP.png)

And in the explorer view, you should now see 2 files added to the project folder

![](https://i.imgur.com/m0jbt33.png)
</details>

## Choosing a new Anvil version

<details>
<summary>6. Click on the file called "<YourPluginName>.csproj", and double check/change to the desired version of Anvil</summary>

![](https://i.imgur.com/zgNWT3q.png)

![](https://i.imgur.com/0G9SR0n.png)
</details>

<details>
<summary>7. Once you have chosen a version, run the following command in the terminal</summary>

```
dotnet restore
```

If you get the following output, the project has been created and is ready for development :)

![](https://i.imgur.com/9EH1eOc.png)

If you get something like the following, make sure you have not made a typo in the version name

![](https://i.imgur.com/01FaFqg.png)
</details>

## Creating your first build

<details>
<summary>8. If you have not already, re-open your project folder using the "Open Folder" dialog</summary>

![](https://i.imgur.com/U1CX26g.png)
</details>

<details>
<summary>9. Open the terminal again, and run the following command</summary>

```
dotnet build
```

This will build the source files in your project, and produce a plugin binary.

You should see the following terminal output:

![](https://i.imgur.com/4PJb6UZ.png)

Once the build has succeeded, you should see a new directory called `bin` containing the compiled plugin binaries.

</details>

<details>
<summary>10. Click "Reveal in File Explorer" on the bin folder to navigate to it.</summary>

![](https://i.imgur.com/rPbPsPg.png)

When submitting plugins for review in the [plugins forum](https://github.com/nwn-dotnet/Anvil/discussions/categories/plugins), copying the plugin to a server, or sharing the plugin with other people, you will need to send the whole folder for the plugin to work.

![](https://i.imgur.com/myeazf0.png)
</details>

<details>
<summary>11. Copy the "YourPluginName" folder to the Anvil Plugin directory. This directory should be inside your NWN home directory (modules/hak/etc), under `anvil/plugins`</summary>

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
<summary>12. Launch the server! If everything is done correctly, you should see the plugin loading as a message</summary>

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
