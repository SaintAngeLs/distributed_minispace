#!/bin/bash
export ASPNETCORE_ENVIRONMENT=local
export NTRADA_CONFIG=ntrada-async
cd src/MiniSpace.Services.Comments.Api
dotnet run
