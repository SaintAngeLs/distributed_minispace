#!/bin/bash

export ASPNETCORE_ENVIRONMENT=docker

cd ..

docker build -t minispace.apigateway:latest .

docker tag minispace.apigateway:latest adrianvsaint/minispace.apigateway:latest

docker push adrianvsaint/minispace.apigateway:latest
