#!/bin/bash

K6_HOSTNAME=api.lotto.dlindemann.xyz
k6 run -e APP_HOSTNAME=$K6_HOSTNAME lottonumbers.js

