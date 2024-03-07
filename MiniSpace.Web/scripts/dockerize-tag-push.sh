#!/bin/bash

export ASPNETCORE_ENVIRONMENT=Development

cd MiniSpace.APIGateway

docker build -t minispace.web:latest .

docker tag minispace.web:latest adrianvsaint/minispace.web:latest

docker push adrianvsaint/minispace.web:latest
