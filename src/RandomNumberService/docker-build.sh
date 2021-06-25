#!/bin/bash

docker build -t dlindemann/randomnumberservice:0.0.1 -t dlindemann/randomnumberservice:latest $(dirname "$0")
