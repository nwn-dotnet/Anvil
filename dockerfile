# Configure nwserver to run with nwnx
FROM nwnxee/unified:latest
ARG BINARY_PATH
COPY ${BINARY_PATH} /nwn/anvil/

# Define we are an Anvil based container
ENV NWN_ANVIL=1

# Set which kill signal to exit upon
STOPSIGNAL SIGINT

# Enable and configure DotNET plugins + dependencies
ENV NWNX_DOTNET_SKIP=n
ENV NWNX_SWIG_DOTNET_SKIP=n
ENV NWNX_UTIL_SKIP=n
ENV NWNX_OBJECT_SKIP=n
ENV NWNX_DOTNET_ASSEMBLY=/nwn/anvil/NWN.Anvil
