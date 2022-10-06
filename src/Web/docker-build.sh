#!/bin/bash

docker build -t dlindemann/lottoweb:1.0.0 -t dlindemann/lottoweb:latest $(dirname "$0")
