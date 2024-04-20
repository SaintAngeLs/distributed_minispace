#!/bin/bash
export ASPNETCORE_ENVIRONMENT=local
cd ../src/MiniSpace.Services.Events.Api
dotnet build -c release