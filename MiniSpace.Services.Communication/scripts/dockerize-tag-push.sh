#!/bin/bash

export ASPNETCORE_ENVIRONMENT=docker

cd ..

docker build -t minispace.services.communication:latest .

docker tag minispace.services.communication:latest adrianvsaint/minispace.services.communication:latest

docker push adrianvsaint/minispace.services.communication:latest
