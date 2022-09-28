#!/bin/bash

docker build -t dlindemann/lottoweb:0.0.1 -t dlindemann/lottoweb:latest $(dirname "$0")
