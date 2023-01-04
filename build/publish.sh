#!/bin/bash

set -e

BUILD_CONFIGURATION="${BUILD_CONFIGURATION:-Release}"
ENVIRONMENT_NAME="${ASPNETCORE_ENVIRONMENT}"

source build-all.sh

PUBLISH_PATH="${BASE_PATH}/publish"

slnContent=$(cat "$BASE_SOLUTION_PATH")

dotnet restore "${BASE_SOLUTION_PATH}" --configfile "${NUGET_FILE}" -nowarn:msb3202,nu1503

for f in $(find "$BASE_PATH" -iwholename '*.csproj'); do
  IFS='/' read -r -a project_paths <<<"$f"

  PROJECT_FILENAME="${project_paths[-1]}"
  PROJECT_NAME="${PROJECT_FILENAME/.csproj/}"

  if [[ ! $slnContent == *"$PROJECT_FILENAME"* ]]; then
    continue
  fi

  for e in "${EXCLUDED_PROJECT_PATHS[@]}"; do
    if [[ "$f" == *"$e"* ]]; then
      unset f
    fi
  done

  if [ ! "$f" ]; then
    continue
  fi

  PROJECT_DIR=$(dirname "$f")
  PACKAGE_JSON="${PROJECT_DIR}/package.json"

  if [ -f "${PACKAGE_JSON}" ]; then
    if grep -q "\"build\"" "${PACKAGE_JSON}"; then
      yarn --cwd "${PROJECT_DIR}" run build
    fi
  fi

  IFS='.' read -r -a project_name_parts <<<"$PROJECT_NAME"
  PROJECT_TARGET_DIR="${project_name_parts[0]}.${project_name_parts[1]}"

  dotnet publish "$f" --no-restore --no-build -nowarn:msb3202,nu1503 -c Release -o "${PUBLISH_PATH}/${PROJECT_TARGET_DIR}" -p:EnvironmentName="${ENVIRONMENT_NAME}"
done
