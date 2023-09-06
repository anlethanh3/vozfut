#!/bin/bash
# file: build-docker.sh
npm install
npm run build
docker build -t fut-web .
docker image tag fut-web:latest anlt/vozfut-web:latest
# docker image push anlt/vozfut-web:latest