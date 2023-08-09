#!/bin/bash
# file: build-docker.sh
dotnet publish -c Release
docker build -t fut-webapi .
docker run -it --rm -p 5000:80 --name fut fut-webapi