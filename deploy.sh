#!/bin/bash

# Check if the GitLab token is provided
if [ -z "$1" ]; then
  echo "Error: GITLAB_TOKEN is not provided"
  exit 1
fi

GITLAB_TOKEN=$1

echo "Using GITLAB_TOKEN: ${GITLAB_TOKEN}"

# Function to pull images and clone settings repository
function setup_environment() {
    echo "Pulling latest images and cloning settings repository..."

    # Pull images for all services
    docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/api-gateway:latest
    docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/web:latest
    docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/services-identity:latest
    docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/services-events:latest
    docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/services-students:latest
    docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/services-friends:latest
    docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/services-reactions:latest
    docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/services-posts:latest
    docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/services-comments:latest
    docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/services-mediafiles:latest
    docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/services-notifications:latest
    docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/services-reports:latest
    docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/services-email:latest
    docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/services-organizations:latest

    # Clone the settings repository
    git clone https://oauth2:${GITLAB_TOKEN}@gitlab.com/distributed-asp-net-core-blazor-social-app/events_public_settings.git /tmp/events_public_settings
}

function copy_configuration() {
    local service=$1
    local source_path=$2
    local destination_path=$3

    echo "Setting up configuration for $service..."

    mkdir -p "${destination_path}"
    cp "${source_path}/appsettings.json" "${destination_path}"
    cp "${source_path}/appsettings.docker.json" "${destination_path}"
}

# Setup environment
setup_environment

# Copy configurations to respective directories
copy_configuration "API Gateway" "/tmp/events_public_settings/APIGateway" "/root/social_net_app/MiniSpace.APIGateway/src/MiniSpace.APIGateway"
copy_configuration "Services Comments" "/tmp/events_public_settings/Services.Comments" "/root/social_net_app/MiniSpace.Services.Comments/src/MiniSpace.Services.Comments.Api"
copy_configuration "Services Email" "/tmp/events_public_settings/Services.Email" "/root/social_net_app/MiniSpace.Services.Email/src/MiniSpace.Services.Email.Api"
copy_configuration "Services Events" "/tmp/events_public_settings/Services.Events" "/root/social_net_app/MiniSpace.Services.Events/src/MiniSpace.Services.Events.Api"
copy_configuration "Services Friends" "/tmp/events_public_settings/Services.Friends" "/root/social_net_app/MiniSpace.Services.Friends/src/MiniSpace.Services.Friends.Api"
copy_configuration "Services Identity" "/tmp/events_public_settings/Services.Identity" "/root/social_net_app/MiniSpace.Services.Identity/src/MiniSpace.Services.Identity.Api"
copy_configuration "Services MediaFiles" "/tmp/events_public_settings/Services.MediaFiles" "/root/social_net_app/MiniSpace.Services.MediaFiles/src/MiniSpace.Services.MediaFiles.Api"
copy_configuration "Services Notifications" "/tmp/events_public_settings/Services.Notifications" "/root/social_net_app/MiniSpace.Services.Notifications/src/MiniSpace.Services.Notifications.Api"
copy_configuration "Services Organizations" "/tmp/events_public_settings/Services.Organizations" "/root/social_net_app/MiniSpace.Services.Organizations/src/MiniSpace.Services.Organizations.Api"
copy_configuration "Services Posts" "/tmp/events_public_settings/Services.Posts" "/root/social_net_app/MiniSpace.Services.Posts/src/MiniSpace.Services.Posts.Api"

echo "Deployment completed successfully."
