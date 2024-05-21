#!/bin/bash

export ASPNETCORE_ENVIRONMENT=docker

cd ..

docker build -t minispace.services.reports:latest .

docker tag minispace.services.reports:latest adrianvsaint/minispace.services.reports:latest

docker push adrianvsaint/minispace.services.reports:latest
