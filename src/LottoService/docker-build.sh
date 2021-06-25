#!/bin/bash

docker build -t dlindemann/lottoservice:0.0.1 -t dlindemann/lottoservice:latest $(dirname "$0")
