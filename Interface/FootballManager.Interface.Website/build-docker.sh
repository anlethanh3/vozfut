#!/bin/bash
# file: build-docker.sh
npm run build
docker build -t fut-web .