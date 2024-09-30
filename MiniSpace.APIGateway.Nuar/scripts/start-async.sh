#!/bin/bash
export ASPNETCORE_ENVIRONMENT=local
export NTRADA_CONFIG=ntrada-async
cd src/MiniSpace.APIGateway
dotnet run
