#!/bin/bash

export ASPNETCORE_ENVIRONMENT=Development

cd ..

docker build -t minispace.services.events:latest .

docker tag minispace.services.events:latest adrianvsaint/minispace.services.events:latest

docker push adrianvsaint/minispace.services.events:latest
