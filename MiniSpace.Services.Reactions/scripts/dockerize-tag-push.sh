#!/bin/bash

export ASPNETCORE_ENVIRONMENT=docker

cd ..

docker build -t minispace.services.reactions:latest .

docker tag minispace.services.reactions:latest adrianvsaint/minispace.services.reactions:latest

docker push adrianvsaint/minispace.services.reactions:latest
