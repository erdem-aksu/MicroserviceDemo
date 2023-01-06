#!/bin/bash

EXCLUDED_PATHS=(
    "/node_modules/"
    "/docker/"
)

for f in $(find ../ -type d -name 'bin' -o -name 'obj'); do
    for e in "${EXCLUDED_PATHS[@]}"; do
        if [[ "$f" == *"$e"* ]]; then
            echo "Skipping: ${f}"
            unset f
        fi
    done

    if [ "$f" ]; then
        echo "Deleting: ${f}"
        rm -fr "$f"
    fi
done
