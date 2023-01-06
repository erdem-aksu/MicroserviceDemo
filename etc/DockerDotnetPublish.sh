#!/bin/bash

CURRENT_PATH=$(realpath "../")

docker run -ti --rm -v "${CURRENT_PATH}":/src mcr.microsoft.com/dotnet/sdk:6.0-focal bash -c "cd /src/build && bash publish.sh"
