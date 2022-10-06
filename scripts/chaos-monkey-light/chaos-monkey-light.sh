#!/bin/bash

SLEEP_SECONDS=${1:-30}
NAMESPACE=${2:-default}
COUNT=${3:-2}

echo "Killing $COUNT pods every $SLEEP_SECONDS seconds from namespace $NAMESPACE"
while [ true ]
do
    kubectl get pods -o name -n $NAMESPACE | tail -$COUNT | xargs -L1 kubectl -n $NAMESPACE delete --wait=false
    sleep $SLEEP_SECONDS
done
