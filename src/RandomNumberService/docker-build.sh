#!/bin/bash

docker build -t dlindemann/randomnumberservice:1.0.0 -t dlindemann/randomnumberservice:latest $(dirname "$0")
