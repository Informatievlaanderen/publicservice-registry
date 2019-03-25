#!/usr/bin/env bash

DOCKER_COMPOSE_VERSION=1.19.0

# Disable dotnet cli telemetry
export DOTNET_CLI_TELEMETRY_OPTOUT=1

sudo pacman --noconfirm -Sy docker docker-compose mono python python-pip aws-cli dotnet-runtime dotnet-sdk

# Print debug information
docker -v
docker-compose -v
mono -V
python3 --version
pip --version
aws --version
dotnet --version

