#!/bin/bash

set -e

BASE_PATH=$(realpath "../")
BASE_SOLUTION_PATH="${BASE_PATH}/MicroserviceDemo.sln"
NUGET_FILE="${BASE_PATH}/NuGet.Config"
PUBLISH_PATH="${BASE_PATH}/publish"
SOLUTION_PATHS=(
  "services/administration"
  "services/identity"
  "services/contact"
  "services/report"
  "gateways/web"
  "apps/auth-server"
  "apps/web"
  "."
)
EXCLUDED_PROJECT_PATHS=(
  "test"
  "shared"
)
PUBLISH_PATH="${BASE_PATH}/publish"
