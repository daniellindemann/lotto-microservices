#!/bin/sh

registry='localhost:4999'
version=0.0.1

# build projects
$(dirname "$0")/../../src/RandomNumberService/docker-build.sh
$(dirname "$0")/../../src/LottoService/docker-build.sh
$(dirname "$0")/../../src/Web/docker-build.sh

# tag the images for local registry
docker tag dlindemann/randomnumberservice $registry/dlindemann/randomnumberservice:$version
docker tag dlindemann/lottoservice $registry/dlindemann/lottoservice:$version
docker tag dlindemann/lottoweb $registry/dlindemann/lottoweb:$version

# push the images
docker push $registry/dlindemann/randomnumberservice --all-tags
docker push $registry/dlindemann/lottoservice --all-tags
docker push $registry/dlindemann/lottoweb --all-tags
