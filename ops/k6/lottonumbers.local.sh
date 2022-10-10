#!/bin/bash

K6_HOSTNAME=host.docker.internal:8081
k6 run -e APP_HOSTNAME=$K6_HOSTNAME lottonumbers.js

