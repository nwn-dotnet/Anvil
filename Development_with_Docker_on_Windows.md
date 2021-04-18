# Running Anvil in docker on windows

To get Anvil up an running for development and testing, the easiest way
is to run it in docker (which is can also be used in Docker with WSL).

#### Why this guide?
Pragmatic, opionated way to setup a Anvil development environment.

#### Preperation
There are some things that you need to install, which is not part of this guide:

- Docker on WSL, see https://docs.docker.com/docker-for-windows/wsl/
- .NET Core SDK 5.0 or above, see https://dotnet.microsoft.com/download ("Download .NET SDK x64")
- An IDE of your choice
    - Jetbrains Rider, see https://www.jetbrains.com/rider/ (thats what I am using)
    - VS Code, see https://code.visualstudio.com/
    - Visual Studio, see https://visualstudio.microsoft.com/de/

#### Notes

This guide utilises the `C:\Users\<USER>\Documents\Neverwinter Nights` folder directly. This has the advantage that all changes
made by the toolset and IDE will immediately be picked up when you restart your docker based server. This provides a fast code-test-cycle.


#### Steps
0. **Create a backup** of `C:\Users\<USER>\Documents\Neverwinter Nights` to a location of your choice (just in case).
1. Open `C:\Users\<USER>\Documents\Neverwinter Nights`. I will refer to this folder as `root\` from now on.
2. Download and extract the latest `NWN.Anvil.zip` from github: https://github.com/nwn-dotnet/Anvil/releases to `root\Anvil`
3. Clone https://github.com/nwn-dotnet/NWN.Samples/tree/master/managed/plugin-sample to `root\plugin-sample` (ATTENTION: `plugin-sample` is NOT the root of the repository but a subfolder. You have to clone to a different location first and copy the appropriate folder to the described location)
4. Open `root\plugin-sample\plugin-sample.csproj` in your IDE
5. Compile the project using the IDE, make sure that `root\plugin-sample\bin\Debug\Plugins\plugin-sample\plugin-sample.dll` is created
6. Create the following file `root/docker-compose.yml` with content. Replace `NWN_MODULE` with the name of your module from `root\modules`.
   In this example I am using the module with filename `module000.mod`.

````yml
version: '3'
services:
  nwn-server:
    image: nwnxee/unified:latest
    ports:
      - 5121:5121/udp
    environment:
      - NWN_PORT=5121
      - NWN_MODULE=module000
      - NWN_PUBLICSERVER=0
      - NWNX_DOTNET_SKIP=n
      - NWNX_OBJECT_SKIP=n
      - NWNX_UTIL_SKIP=n
      - NWNX_DOTNET_ASSEMBLY=/Anvil/NWN.Anvil
    volumes:
      - ./modules:/nwn/home/modules
      - ./hak:/nwn/home/hak
      - ./tlk:/nwn/home/tlk
      - ./Anvil:/Anvil
      - ./plugin-sample/bin/Debug/Plugins:/Anvil/Plugins
````

7. Your `C:\Users\<USER>\Documents\Neverwinter Nights` should now look like this:

````
├── ambient\
├── database\
├── development\
├── dmvault\
├── hak\
├── localvault\
├── logs\
├── modules\
├── movies\
├── music\
├── Anvil\              <-- Added by you
│   │   ├── Plugins
│   │   │   ├── [empty, no files]
│   ├── NWN.Anvil.dll
│   ├── [...]
├── nwsync\
├── override\
├── plugin-sample\            <-- Added by you
│   │   ├── bin
│   │   │   ├── Debug
│   │   │   │   ├── Plugins
│   ├── plugin-sample.csproj
│   ├── [...]
├── portraits\
├── servervault\
├── temp\
├── tempclient\
├── tlk\
├── cdkey.ini
├── cryptographic_secret
├── docker-compose.yml        <-- Added by you
├── nwn.ini
├── nwnplayer.ini
├── nwtoolset.ini
├── settings.tml
````

8. Run `docker-compose up` and look for errors in the output. Make sure your module is loaded:

````log
[...]
nwn-server_1  | I [2021/01/17 16:24:30.668] [NWN.Services.ServiceInstaller] Registered service: Sample.MyPluginService  <<--- Look for this line
nwn-server_1  |
nwn-server_1  | Server: Module loaded
nwn-server_1  | stdin closed, not accepting interactive commands.
nwn-server_1  | I [16:24:33] [NWNX_ServerLogRedirector] [ServerLogRedirector.cpp:75] (Server) Our public address as seen by the masterserver: XX.XX.XX.XX:XXX
````   

9. Connect to your server in game: `Multiplayer --> LAN-Game --> Direct Connect --> use 127.0.0.1:5121, no Password --> Enter any name you like --> Create or select Character --> Play`
