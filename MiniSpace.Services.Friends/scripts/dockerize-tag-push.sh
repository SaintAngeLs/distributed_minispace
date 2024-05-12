#!/bin/bash

export ASPNETCORE_ENVIRONMENT=docker

cd ..

docker build -t minispace.services.friends:latest .

docker tag minispace.services.friends:latest adrianvsaint/minispace.services.friends:latest

docker push adrianvsaint/minispace.services.friends:latest
