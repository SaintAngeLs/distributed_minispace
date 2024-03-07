#!/bin/bash

export ASPNETCORE_ENVIRONMENT=Development

cd ..

docker build -t minispace.services.identity:latest .

docker tag minispace.services.identity:latest adrianvsaint/minispace.services.identity:latest

docker push adrianvsaint/minispace.services.identity:latest
