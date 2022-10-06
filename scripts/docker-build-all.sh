#!/bin/sh

# build projects
$(dirname "$0")/../src/RandomNumberService/docker-build.sh
$(dirname "$0")/../src/LottoService/docker-build.sh
$(dirname "$0")/../src/Web/docker-build.sh
