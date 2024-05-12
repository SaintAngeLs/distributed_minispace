#!/bin/bash

export ASPNETCORE_ENVIRONMENT=docker

cd ..

docker build -t minispace.services.comments:latest .

docker tag minispace.services.comments:latest adrianvsaint/minispace.services.comments:latest

docker push adrianvsaint/minispace.services.comments:latest
