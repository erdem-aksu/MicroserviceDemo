#!/bin/bash

touch ./traefik/acme.json
chmod -R 600 ./traefik/acme.json

docker-compose --env-file ../.env -f docker/docker-compose.infrastructure.yml -f docker/docker-compose.infrastructure.override.yml -f docker/docker-compose.yml -f docker/docker-compose.override.yml build --build-arg CACHEBUST="$(date +%s)"
