#!/bin/bash

export ASPNETCORE_ENVIRONMENT=docker

cd ..

docker build -t minispace.services.mediafiles:latest .

docker tag minispace.services.mediafiles:latest adrianvsaint/minispace.services.mediafiles:latest

docker push adrianvsaint/minispace.services.mediafiles:latest
