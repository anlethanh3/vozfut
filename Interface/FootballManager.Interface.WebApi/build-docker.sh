#!/bin/bash
# file: build-docker.sh
dotnet publish -c Release
docker build -t fut-webapi .