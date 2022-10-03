# See here for image contents: https://github.com/microsoft/vscode-dev-containers/tree/v0.245.2/containers/dotnet/.devcontainer/base.Dockerfile

# [Choice] .NET version: 6.0, 3.1, 6.0-bullseye, 3.1-bullseye, 6.0-focal, 3.1-focal
ARG VARIANT="6.0-bullseye-slim"
FROM mcr.microsoft.com/vscode/devcontainers/dotnet:0-${VARIANT}

# [Choice] Node.js version: none, lts/*, 18, 16, 14
ARG NODE_VERSION="none"
RUN if [ "${NODE_VERSION}" != "none" ]; then su vscode -c "umask 0002 && . /usr/local/share/nvm/nvm.sh && nvm install ${NODE_VERSION} 2>&1"; fi

# [Optional] Uncomment this section to install additional OS packages.
RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
    && apt-get -y install --no-install-recommends vim

# [Optional] Uncomment this line to install global node packages.
ARG ANGULAR_VERSION="none"
RUN su vscode -c "source /usr/local/share/nvm/nvm.sh && npm install -g @angular/cli@${ANGULAR_VERSION}" 2>&1

# install dotnet tools
ARG TYE_VERSION="none"
RUN if [ "${TYE_VERSION}" != null ]; then su vscode -c "dotnet tool install -g Microsoft.Tye --version \"${TYE_VERSION}\""; fi

# # TODO: check architecture
# ARG DRAFT_VERSION="none"
# RUN if [ "${DRAFT_VERSION}" != null ]; then \
#     unameArch=$(uname --machine) && \
#     if [[ $unameArch==*"aarch64"* ]]; then installArch="arm64"; else installArch="arm64"; fi; \
#     wget --content-disposition "https://github.com/Azure/draft/releases/download/v${DRAFT_VERSION}/draft-linux-$installArch" && mv draft-linux-$installArch draft && chmod +x draft && mv draft /usr/bin/draft; \
#     fi

# install DAPR
RUN wget -q https://raw.githubusercontent.com/dapr/cli/master/install/install.sh -O - | /bin/bash

