#!/bin/bash

# execute draft files
pushd $(dirname "$0")/../src/RandomNumberService && ./draft-create.sh && popd \
    && pushd $(dirname "$pwd")/../src/LottoService && ./draft-create.sh && popd \
    && pushd $(dirname "$pwd")/../src/Web/ && ./draft-create.sh && popd
