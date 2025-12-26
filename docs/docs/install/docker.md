## Prerequisites
### Docker
- For Windows, you can install Docker Desktop from here: https://www.docker.com/get-started
- For Linux servers, you can install Docker via a package manager. See the docker wiki for more info: https://docs.docker.com/engine/install/ubuntu/

## Installing Anvil
1. Paste the following into a text document. Save the file as `docker-compose.yml`:
```yml
services:
  anvil:
    image: nwndotnet/anvil:8193.34.2
    container_name: anvil
    restart: unless-stopped
    stdin_open: true
    tty: true
    volumes:
    - ./server:/nwn/home
    - ./logs:/nwn/run/logs.0
    ports:
    - '5121:5121/udp'
```
2. Change `image: nwndotnet/anvil:8193.34.2` to the appropriate version you wish to run.

The following tags are supported. It is **STRONGLY** recommended to pin your server to a specific version or commit instead of the `latest` tag.

|Tag|Example|
|-|-|
|`latest`|`nwndotnet/anvil:latest`|
|`<version>`|`nwndotnet/anvil:8193.22.1`|
|`<commit-hash>`|`nwndotnet/anvil:c09d2f6`|

3. Open your terminal and execute the following in the same directory you created `docker-compose.yml`
```
docker-compose up
```
4. The server should now be running!

## Configuration
After Anvil has launched for the first time, several new folders will be created:

- `logs` contains log output from the server and anvil services
- `server` contains all data specific to your server.
- `server/anvil` contains all data specific to anvil. In the docs, this is referred to as your **ANVIL HOME** directory.

By default, the server will load a placeholder module. There are a few additional steps involved to begin loading your content:

1. To load your module, you will want to copy your module file to the `server/modules` folder.
-  Don't forget to copy any required hak/tlk files to the `server/hak` & `server/tlk` folders.
2. Open the `docker-compose.yml` file and add the `env-file` section:
```yml
version: '3'
services:
  anvil:
    image: nwndotnet/anvil:8193.34.2
    container_name: anvil
    restart: unless-stopped
    stdin_open: true
    tty: true
    volumes:
    - ./server:/nwn/home
    - ./logs:/nwn/run/logs.0
    ports:
    - '5121:5121/udp'
    env_file:
      - ./nwserver.env
```
3. Paste the following into a text document, and customize as you see fit. Save the file as `nwserver.env` next to your docker-compose file:
```
# The name of your server as listed in the server browser.
NWN_SERVERNAME=Anvil Server
# The name of your module file without the extension (e.g. module file = my_module.mod)
NWN_MODULE=my_module
# 1 if server should be available from the server browser, 0 if LAN only.
NWN_PUBLICSERVER=1
NWN_PLAYERPASSWORD=
NWN_DMPASSWORD=
NWN_ADMINPASSWORD=

NWN_PORT=5121
NWN_MAXCLIENTS=16
NWN_MINLEVEL=1
NWN_MAXLEVEL=40
NWN_PAUSEANDPLAY=0
# 0: None, 1: Party, 2: Full
NWN_PVP=2
NWN_SERVERVAULT=1
# Enforce legal characters
NWN_ELC=1
# Item level restrictions
NWN_ILR=0
NWN_GAMETYPE=3
NWN_ONEPARTY=0
NWN_DIFFICULTY=3
NWN_AUTOSAVEINTERVAL=0
NWN_RELOADWHENEMPTY=0

# Redirect server logs to Anvil, so it can be captured and persisted by `anvil.log`
ANVIL_LOG_MODE=Redirect
```
- This is just a subset of available configuration options. See the [NWNX](https://nwnxee.github.io/unified/) and [NWServer](https://hub.docker.com/r/urothis/nwserver#run-configuration-base-image) README's for further configuration options.

4. The server should now be running your module!
