#!/bin/bash
# file: build-docker.sh
dotnet publish -c Release
docker build -t fut-webapi .
docker image tag fut-webapi:latest anlt/vozfut-webapi:latest
docker image push anlt/vozfut-webapi:latest