# Load nwnx image to import nwserver + nwnx plugins
FROM nwnxee/unified:a6c5f09 as nwnx

# Remove incompatible plugins
RUN rm -rf /nwn/nwnx/NWNX_Ruby.so \
    /nwn/nwnx/NWNX_SpellChecker.so \
    /nwn/nwnx/NWNX_Redis.so

FROM ubuntu:20.04

COPY --from=nwnx /nwn /nwn

RUN apt-get update \
&& apt-get --no-install-recommends -y install ca-certificates wget \
&& wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
&& dpkg -i packages-microsoft-prod.deb \
&& rm packages-microsoft-prod.deb \
&& apt-get update \
&& apt-get --no-install-recommends -y install libc6 libstdc++6 \
    hunspell \
    default-libmysqlclient-dev \
    libmariadb3 \
    libpq5 \
    libsqlite3-0 \
    luajit libluajit-5.1-2 \
    inotify-tools \
    patch \
    unzip \
    dotnet-runtime-8.0 \
    dotnet-apphost-pack-8.0 \
&& rm -rf /var/cache/apt /var/lib/apt/lists/*

# Copy Anvil Binaries
ARG BINARY_PATH
COPY ${BINARY_PATH} /nwn/anvil/

# Define we are an Anvil based container
ENV ANVIL_IMAGE=1

# Set which kill signal to exit upon
STOPSIGNAL SIGINT

# Patch run-server.sh with our modifications
COPY ./scripts/run-server.patch /nwn
RUN patch /nwn/run-server.sh < /nwn/run-server.patch

# User Data
VOLUME /nwn/home

# Configure nwserver to run with nwnx
ENV NWNX_CORE_LOAD_PATH=/nwn/nwnx/
ENV NWN_LD_PRELOAD="/nwn/nwnx/NWNX_Core.so"

# Configure nwnx to run with anvil
ENV NWNX_DOTNET_SKIP=n
ENV NWNX_SWIG_DOTNET_SKIP=n
ENV NWNX_DOTNET_ASSEMBLY=/nwn/anvil/NWN.Anvil
ENV NWNX_DOTNET_ENTRYPOINT=Anvil.AnvilCore
ENV NWNX_DOTNET_METHOD=Bootstrap
ENV NWNX_DOTNET_NEW_BOOTSTRAP=true

# Use NWNX_ServerLogRedirector as default log manager
ENV NWNX_SERVERLOGREDIRECTOR_SKIP=n
ENV NWN_TAIL_LOGS=n
ENV NWNX_CORE_LOG_LEVEL=6
ENV NWNX_SERVERLOGREDIRECTOR_LOG_LEVEL=6

# Disable all other plugins by default.
ENV NWNX_CORE_SKIP_ALL=y

# Enable minidump generation
ENV DOTNET_DbgEnableMiniDump=1
ENV DOTNET_DbgMiniDumpType=3
ENV DOTNET_CreateDumpDiagnostics=1
ENV DOTNET_CreateDumpVerboseDiagnostics=1
ENV DOTNET_DbgMiniDumpName=/nwn/run/logs.0/anvil-crash-%t.dmp
ENV DOTNET_CreateDumpLogToFile=/nwn/run/logs.0/anvil-crash.log

# Entrypoint & Executable
EXPOSE ${NWN_PORT:-5121}/udp

RUN chmod +x /nwn/data/bin/linux-amd64/nwserver
RUN chmod +x /nwn/run-server.sh
ENV NWN_EXTRA_ARGS="-userdirectory /nwn/run"

WORKDIR /nwn/data/bin/linux-amd64
ENTRYPOINT ["/bin/bash", "/nwn/run-server.sh"]
