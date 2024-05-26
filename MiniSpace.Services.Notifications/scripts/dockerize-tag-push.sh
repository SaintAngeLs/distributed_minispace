#!/bin/bash

export ASPNETCORE_ENVIRONMENT=docker

cd ..

docker build -t minispace.services.notifications:latest .

docker tag minispace.services.notifications:latest adrianvsaint/minispace.services.notifications:latest

docker push adrianvsaint/minispace.services.notifications:latest
