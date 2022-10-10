#!/bin/bash

# dotnet restore
dotnet restore

# npm install for webapp, so all dependencies are already before the first run of Web
cd ./src/Web/ClientApp && npm i && cd ../../..

# install cert with new certifcate path
# certpath=$(echo $TEMP_ASPNETCORE_Kestrel__Certificates__Default__Path | cut -d'/' -f4-)    # replace /home/<user> with /home/vscode
# export ASPNETCORE_Kestrel__Certificates__Default__Path="$HOME/$certpath"
dotnet dev-certs https --clean --import "$ASPNETCORE_Kestrel__Certificates__Default__Path" --password "$ASPNETCORE_Kestrel__Certificates__Default__Password"

# dapr init
dapr uninstall --all
dapr init

# setup mongo container
MONGO_CONTAINER_NAME=lotto_mongo
MONGO_VERSION=6.0.2
if [ ! "$(docker ps -a | grep $MONGO_CONTAINER_NAME)" ]; then
    docker run -d --name $MONGO_CONTAINER_NAME -p 27017:27017 mongo:$MONGO_VERSION
fi
if [ "$(docker container inspect -f '{{.State.Status}}' $MONGO_CONTAINER_NAME)" != "running" ]; then
    docker start $MONGO_CONTAINER_NAME
fi
