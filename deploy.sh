#!/bin/bash

# Check if the GitLab token is provided
if [ -z "$1" ]; then
  echo "Error: GITLAB_TOKEN is not provided"
  exit 1
fi

GITLAB_TOKEN=$1
DOCKER_COMPOSE_FILE="/root/social_net_app/services-prod.yml"

echo "Using GITLAB_TOKEN: ${GITLAB_TOKEN}"

# Function to pull images and clone settings repository
function setup_environment() {
    echo "Pulling latest images and cloning settings repository..."

    # List all the services and pull their images from GitLab container registry
    declare -a services=("api-gateway" 
    "web" 
    "services-identity" 
    "services-events" 
    "services-students" 
    "services-friends" 
    "services-reactions" 
    "services-posts" 
    "services-comments" 
    "services-mediafiles" 
    "services-notifications" 
    "services-reports" 
    # "service-organizations"
    "services-email")

    for service in "${services[@]}"
    do
        if docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/$service:latest; then
            echo "$service image pulled successfully"
        else
            echo "Warning: $service image not found"
        fi
    done

    # Remove any existing settings directory and clone the new one
    rm -rf /tmp/events_public_settings
    git clone https://oauth2:${GITLAB_TOKEN}@gitlab.com/distributed-asp-net-core-blazor-social-app/events_public_settings.git /tmp/events_public_settings
}

function copy_configuration() {
    local service_name=$1
    local source_path="/tmp/events_public_settings/${service_name}"
    local destination_path="/root/social_net_app/MiniSpace.${service_name}/src/MiniSpace.${service_name}"

    echo "Setting up configuration for $service_name..."

    # Ensure the destination directory exists and copy the configuration files
    mkdir -p "${destination_path}"
    cp "${source_path}/appsettings.json" "${destination_path}/appsettings.json"
    cp "${source_path}/appsettings.docker.json" "${destination_path}/appsettings.docker.json"
}

# Copy the Docker Compose file to the correct location
if [ ! -f "$DOCKER_COMPOSE_FILE" ]; then
  echo "Error: $DOCKER_COMPOSE_FILE does not exist. Please ensure it is copied to the server."
  exit 1
fi

# Setup environment
setup_environment

# Copy configurations to respective directories
# Specify the exact name of the folder if it differs from the service name
copy_configuration "APIGateway"
copy_configuration "Services.Comments"
copy_configuration "Services.Email"
copy_configuration "Services.Events"
copy_configuration "Services.Friends"
copy_configuration "Services.Identity"
copy_configuration "Services.MediaFiles"
copy_configuration "Services.Notifications"
# copy_configuration "Services.Organizations"
copy_configuration "Services.Posts"
copy_configuration "Services.Reactions"
copy_configuration "Services.Reports"
copy_configuration "Services.Students"

echo "Configurations set. Deploying with Docker Compose..."

# Run Docker Compose to start all services
docker-compose -f "$DOCKER_COMPOSE_FILE" up -d

echo "Deployment completed successfully."
