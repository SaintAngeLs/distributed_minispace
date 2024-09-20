FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . .

RUN dotnet publish src/MiniSpace.Services.Communication.Api -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://*:80
ENV ASPNETCORE_ENVIRONMENT=docker
ENV NTRADA_CONFIG=ntrada.docker

ENTRYPOINT ["dotnet", "MiniSpace.Services.Communication.Api.dll"]