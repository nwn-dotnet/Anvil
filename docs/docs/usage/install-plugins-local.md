## Installing Plugins

When a plugin is built and published, it will create a self-contained folder with a .dll binary that looks something like this:

```
YourPlugin/
|----YourPlugin.dll
|----YourPlugin.pdb
|----YourPlugin.deps.json
|----YourPlugin.runtimeconfig.dev.json
|----YourPlugin.runtimeconfig.json
```

This folder is consider an Anvil "plugin" that can be dropped in to your server to install the features and behaviours it provides.

To install the plugin, simply copy the plugin to your server's `<home>/<anvil_home>/Plugins` folder.

Your server home directory should look like the following after it has been copied:
```
    anvil/
    |----Plugins/
         |----YourPlugin/
              |----YourPlugin.dll
              |----...
              |----...
              |----YourPlugin.runtimeconfig.json
    database/
    development/
    hak/
    modules/
    override/
    portraits/
    saves/
    serverdata/
    servervault/
    tlk/
 ```

## Plugin Development with Docker
If you are running a server in docker, you can mount your plugin project's build output as a volume so it's immediately available when you next start the server without any extra manual copying.

Simply add the following entry to your `docker-compose` file:
```
    volumes:
      - ./server:/nwn/home
      - ./logs:/nwn/run/logs.0
      - c:/path/to/YourPlugin/bin/Debug/YourPlugin:/nwn/run/anvil/Plugins/YourPlugin
```
