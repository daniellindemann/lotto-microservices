#!/bin/bash

docker build -t dlindemann/lottoservice:1.0.0 -t dlindemann/lottoservice:latest $(dirname "$0")
