#!/bin/bash

touch ./etc/traefik/acme.json
chmod -R 600 ./etc/traefik/acme.json

docker-compose --env-file ../.env -f docker-compose.infrastructure.yml -f docker-compose.infrastructure.override.yml -f docker-compose.yml -f docker-compose.override.yml up -d
