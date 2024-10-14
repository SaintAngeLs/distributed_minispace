# Use the .NET 6 SDK for building the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and publish the release
COPY . .
RUN dotnet publish src/MiniSpace.Web -c Release -o out

# Use the .NET 6 runtime for the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published app from the build stage
COPY --from=build /app/out .

# Set environment variables
ENV ASPNETCORE_URLS=http://*:80
ENV ASPNETCORE_ENVIRONMENT=docker

# Start the application
ENTRYPOINT ["dotnet", "MiniSpace.Web.dll"]
