#!/bin/bash

export ASPNETCORE_ENVIRONMENT=docker

cd ..

docker build -t minispace.services.posts:latest .

docker tag minispace.services.posts:latest adrianvsaint/minispace.services.posts:latest

docker push adrianvsaint/minispace.services.posts:latest
