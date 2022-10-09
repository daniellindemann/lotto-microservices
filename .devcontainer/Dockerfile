# See here for image contents: https://github.com/microsoft/vscode-dev-containers/tree/v0.245.2/containers/dotnet/.devcontainer/base.Dockerfile

# [Choice] .NET version: 6.0, 3.1, 6.0-bullseye, 3.1-bullseye, 6.0-focal, 3.1-focal
ARG VARIANT="6.0-bullseye-slim"
FROM mcr.microsoft.com/vscode/devcontainers/dotnet:0-${VARIANT}

# [Choice] Node.js version: none, lts/*, 18, 16, 14
ARG NODE_VERSION="none"
RUN if [ "${NODE_VERSION}" != "none" ]; then su vscode -c "umask 0002 && . /usr/local/share/nvm/nvm.sh && nvm install ${NODE_VERSION} 2>&1"; fi

# [Optional] Uncomment this section to install additional OS packages.
ARG ADDITIONAL_APT_PACKAGES="none"
RUN if [ "${ADDITIONAL_APT_PACKAGES}" != "none" ]; then \
    apt-get update && export DEBIAN_FRONTEND=noninteractive \
        && apt-get -y install --no-install-recommends $ADDITIONAL_APT_PACKAGES; \
    fi

# [Optional] Uncomment this line to install global node packages.
ARG ANGULAR_VERSION="none"
RUN if [ "${ANGULAR_VERSION}" != "none" ]; then su vscode -c "source /usr/local/share/nvm/nvm.sh && npm install -g @angular/cli@${ANGULAR_VERSION} 2>&1"; fi

# install dotnet tools
ARG TYE_VERSION="none"
RUN if [ "${TYE_VERSION}" != "none" ]; then su vscode -c "dotnet tool install -g Microsoft.Tye --version \"${TYE_VERSION}\" 2>&1"; fi

# install DAPR
ARG DAPR_VERSION="none"
RUN if [ "${DAPR_VERSION}" != "none" ]; then su vscode -c "wget -q https://raw.githubusercontent.com/dapr/cli/master/install/install.sh -O - | /bin/bash 2>&1"; fi

# draft version; "none" or version number
ARG DRAFT_VERSION="0.0.24"
# draft architecture; amd64 or arm64
ARG DRAFT_ARCH="amd64"
# draft platform; linux or darwin
ARG DRAFT_PLATFORM="linux"
# TODO: architecture should be automatically detected for mac users
RUN if [ "${DRAFT_VERSION}" != "none" ]; then \
    curl -O -L "https://github.com/Azure/draft/releases/download/v${DRAFT_VERSION}/draft-${DRAFT_PLATFORM}-${DRAFT_ARCH}" \
        && mv draft-$DRAFT_PLATFORM-$DRAFT_ARCH draft && chmod +x draft && mv draft /usr/bin/draft; \
    fi

# install k9s from binary
ARG K9S_VERSION="none"
RUN if [ "${K9S_VERSION}" != "none" ]; then \
    curl -O -L "https://github.com/derailed/k9s/releases/download/v${K9S_VERSION}/k9s_Linux_x86_64.tar.gz" \
        && mkdir k9s && tar -zxvf k9s_Linux_x86_64.tar.gz --directory k9s && chmod +x ./k9s/k9s && mv ./k9s/k9s /usr/bin/k9s; \
    fi

# install k6 from binary
ARG K6_VERSION="none"
RUN if [ "${K6_VERSION}" != "none" ]; then \
    curl -O -L "https://github.com/grafana/k6/releases/download/v${K6_VERSION}/k6-v${K6_VERSION}-linux-amd64.tar.gz" \
        && tar -zxvf k6-v${K6_VERSION}-linux-amd64.tar.gz && chmod +x ./k6-v${K6_VERSION}-linux-amd64/k6 && mv ./k6-v${K6_VERSION}-linux-amd64/k6 /usr/bin/k6; \
    fi
