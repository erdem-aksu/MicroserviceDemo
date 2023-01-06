#!/bin/bash

set -a
source ../.env
set +a

openssl req -x509 -newkey rsa:4096 -sha256 -nodes -keyout authserver.key -out authserver.crt -subj "//CN=$DEMO_DOMAIN" -days 3650
openssl pkcs12 -export -out authserver.pfx -inkey authserver.key -in authserver.crt
