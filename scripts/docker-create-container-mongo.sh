#!/bin/sh

name=${1:-mongo_lotto}
image=${2:-mongo:6.0.2}
restart_policy=${3:-unless-stopped}

if [ ! "$(docker ps -a | grep $name)" ]; then
    docker run -d --name $name --restart $restart_policy -p 27017:27017 $image
    echo "Container $name created"
fi
if [ "$(docker container inspect -f '{{.State.Status}}' $name)" != "running" ]; then
    docker start $name
    echo "Container $name started"
fi

echo "Container $name OK"
