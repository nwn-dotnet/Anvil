# Configure nwserver to run with nwnx
FROM nwnxee/unified:latest
ARG BINARY_PATH
COPY ${BINARY_PATH} /nwn/nwnm/

# Enable and configure DotNET plugins + dependencies
ENV NWNX_DOTNET_SKIP=n
ENV NWNX_SWIG_DOTNET_SKIP=n
ENV NWNX_UTIL_SKIP=n
ENV NWNX_OBJECT_SKIP=n
ENV NWNX_DOTNET_ASSEMBLY=/nwn/nwnm/NWN.Managed