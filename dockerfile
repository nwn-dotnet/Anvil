# Configure nwserver to run with nwnx
FROM nwnxee/unified:864fb00
ARG BINARY_PATH
COPY ${BINARY_PATH} /nwn/anvil/

# Set up symlinks
RUN ln -s /nwn/home/anvil /nwn/run/anvil

# Define we are an Anvil based container
ENV ANVIL_IMAGE=1

# Set which kill signal to exit upon
STOPSIGNAL SIGINT

# Enable and configure DotNET plugins + dependencies
ENV NWNX_DOTNET_SKIP=n
ENV NWNX_SWIG_DOTNET_SKIP=n
ENV NWNX_DOTNET_ASSEMBLY=/nwn/anvil/NWN.Anvil
