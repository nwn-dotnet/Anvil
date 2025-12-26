## Visual Studio
TODO


## Visual Studio Code
Install the [omnisharp C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp). You'll need the microsoft release of vscode as the OSS version vscodium is not licensed to use omnisharp.

Then follow the [official instructions](https://github.com/OmniSharp/omnisharp-vscode/blob/master/debugger.md).
For remote debugging, eg. using Docker, see [Attaching to remote processes](https://github.com/OmniSharp/omnisharp-vscode/wiki/Attaching-to-remote-processes)

### Hints on remote debugging with Docker
Make sure you have a debuggable image by installing vsdbg. It is recommended to contain the debug files within a separate image from that which you deploy. For instance, create this `Debug.Dockerfile` where `my-image:my-tag` is a deployable image.
```Dockerfile
FROM my-image:my-tag
RUN mkdir -p ~/.ssh
RUN apt-get update && apt-get install -y --no-install-recommends unzip curl procps
RUN curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l /vsdbg
```

Build this image and bring up the debuggable container, mounting in the plugin sources with something like `-v $(pwd)/src/bin/Debug/Plugins/MyPlugin:/nwn/anvil/bin/Plugins/MyPlugin`

Then add the following `launch.json` configuration
```json
{
    "configurations": [
        {
            "name": ".NET Core Docker Attach",
            "type": "coreclr",
            "request": "attach",
            "processId": "${command:pickRemoteProcess}",
            "pipeTransport": {
                "pipeCwd": "${workspaceRoot}",
                "pipeProgram": "docker",
                "pipeArgs": ["exec", "-i", "my-service-name"],
                "debuggerPath": "/vsdbg/vsdbg",
                "quoteArgs": false,
            },
            "sourceFileMap": {
                "/nwn/anvil":"${workspaceRoot}/host/path/to/dotnet/sources",
            },
            "justMyCode": true,
            "symbolOptions": {
                "searchPaths": ["/nwn/anvil/bin/Plugins/MyPlugin"],
                "searchMicrosoftSymbolServer": false,
                "searchNuGetOrgSymbolServer": false
            },
            "requireExactSource": false
        }
    ]
}
```
Note that:
* `my-service-name` must equal the name of the docker container
* `sourceFileMap` tells vscode where to look for the source files using the format `"/absolute/container/path":"relative/host/path"`
* `searchPaths` tells vscode where to look for debug symbols within the container

You may now debug the `nwserver-linux` process within the container.


## Rider
Not supported at the time of writing (14.07.2021).

```
Rider's debugger does not support the following types of applications:
*  Single-file applications — applications that were built with the /p:PublishSingleFile=true flag.

*  Native applications that host CLR to run .NET code. For example, applications that use CLR Hosting.
```
[Source](https://www.jetbrains.com/help/rider/Debugging_Code.html)
