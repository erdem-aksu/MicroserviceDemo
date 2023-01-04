#!/bin/bash

set -e

source common.sh

for f in "${SOLUTION_PATHS[@]}"; do
    cd "${BASE_PATH}/${f}"
    dotnet restore --configfile "${NUGET_FILE}"
    dotnet build --no-restore -c "${BUILD_CONFIGURATION:-Debug}"
done

cd "$BASE_PATH"
