#!/bin/bash

export ASPNETCORE_ENVIRONMENT=Development

cd ..

docker build -t minispace.services.students:latest .

docker tag minispace.services.students:latest adrianvsaint/minispace.services.students:latest

docker push adrianvsaint/minispace.services.students:latest
