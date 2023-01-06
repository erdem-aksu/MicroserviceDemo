#!/bin/bash

set -e

cd /app

exec dotnet "$DOTNET_APP".dll
