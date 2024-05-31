#!/bin/bash

export ASPNETCORE_ENVIRONMENT=docker

cd ..

docker build -t minispace.services.email:latest .

docker tag minispace.services.email:latest adrianvsaint/minispace.services.email:latest

docker push adrianvsaint/minispace.services.email:latest
