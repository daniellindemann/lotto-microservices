#!/bin/bash

dotnetDirs=( "bin" "obj" )
for i in "${dotnetDirs[@]}"
do
    find $(dirname "$0") -type d -name $i -exec rm -rfv {} \;
done
