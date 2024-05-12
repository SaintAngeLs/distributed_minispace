#!/bin/bash

export ASPNETCORE_ENVIRONMENT=docker

cd ..

docker build -t minispace.services.organizations:latest .

docker tag minispace.services.organizations:latest adrianvsaint/minispace.services.organizations:latest

docker push adrianvsaint/minispace.services.organizations:latest
