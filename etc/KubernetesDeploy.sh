#!/bin/bash

set -a
source ../.env
set +a

cd k8s && bash Deploy.sh
