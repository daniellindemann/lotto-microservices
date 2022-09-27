#!/bin/bash

# dotnet restore
dotnet restore

# npm install for webapp, so all dependencies are already before the first run of Web
cd ./src/Web/ClientApp && npm i && cd ../../..

# install cert with new certifcate path
# certpath=$(echo $TEMP_ASPNETCORE_Kestrel__Certificates__Default__Path | cut -d'/' -f4-)    # replace /home/<user> with /home/vscode
# export ASPNETCORE_Kestrel__Certificates__Default__Path="$HOME/$certpath"
dotnet dev-certs https --clean --import "$ASPNETCORE_Kestrel__Certificates__Default__Path" --password "$ASPNETCORE_Kestrel__Certificates__Default__Password"