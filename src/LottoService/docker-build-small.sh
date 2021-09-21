#!/bin/bash

docker build -t dlindemann/lottoservice-small:0.0.1 -t dlindemann/lottoservice-small:latest -f Dockerfile-small $(dirname "$0")
