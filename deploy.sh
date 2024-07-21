#!/bin/bash

if [ -z "$1" ]; then
  echo "GITLAB_TOKEN is not provided"
  exit 1
fi

GITLAB_TOKEN=$1


# Pull the latest images
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
# docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/services-organizations:latest
docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/services-notifications:latest
docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/services-reports:latest
docker pull registry.gitlab.com/distributed-asp-net-core-blazor-social-app/distributed_minispace/services-email:latest

echo "Using GITLAB_TOKEN: ${GITLAB_TOKEN}"
# Clone the appsettings repository
git clone https://oauth2:${GITLAB_TOKEN}@gitlab.com/distributed-asp-net-core-blazor-social-app/events_public_settings.git /tmp/events_public_settings

# Copy the appsettings to the correct locations
cp /tmp/events_public_settings/APIGateway/appsettings.json /root/social_net_app/MiniSpace.APIGateway/src/MiniSpace.APIGateway/
cp /tmp/events_public_settings/APIGateway/appsettings.docker.json /root/social_net_app/MiniSpace.APIGateway/src/MiniSpace.APIGateway/

cp /tmp/events_public_settings/Services.Comments/appsettings.json /root/social_net_app/MiniSpace.Services.Comments/src/MiniSpace.Services.Comments.Api/
cp /tmp/events_public_settings/Services.Comments/appsettings.docker.json /root/social_net_app/MiniSpace.Services.Comments/src/MiniSpace.Services.Comments.Api/

cp /tmp/events_public_settings/Services.Email/appsettings.json /root/social_net_app/MiniSpace.Services.Email/src/MiniSpace.Services.Email.Api/
cp /tmp/events_public_settings/Services.Email/appsettings.docker.json /root/social_net_app/MiniSpace.Services.Email/src/MiniSpace.Services.Email.Api/

cp /tmp/events_public_settings/Services.Events/appsettings.json /root/social_net_app/MiniSpace.Services.Events/src/MiniSpace.Services.Events.Api/
cp /tmp/events_public_settings/Services.Events/appsettings.docker.json /root/social_net_app/MiniSpace.Services.Events/src/MiniSpace.Services.Events.Api/

cp /tmp/events_public_settings/Services.Friends/appsettings.json /root/social_net_app/MiniSpace.Services.Friends/src/MiniSpace.Services.Friends.Api/
cp /tmp/events_public_settings/Services.Friends/appsettings.docker.json /root/social_net_app/MiniSpace.Services.Friends/src/MiniSpace.Services.Friends.Api/

cp /tmp/events_public_settings/Services.Identity/appsettings.json /root/social_net_app/MiniSpace.Services.Identity/src/MiniSpace.Services.Identity.Api/
cp /tmp/events_public_settings/Services.Identity/appsettings.docker.json /root/social_net_app/MiniSpace.Services.Identity/src/MiniSpace.Services.Identity.Api/

cp /tmp/events_public_settings/Services.MediaFiles/appsettings.json /root/social_net_app/MiniSpace.Services.MediaFiles/src/MiniSpace.Services.MediaFiles.Api/
cp /tmp/events_public_settings/Services.MediaFiles/appsettings.docker.json /root/social_net_app/MiniSpace.Services.MediaFiles/src/MiniSpace.Services.MediaFiles.Api/

cp /tmp/events_public_settings/Services.Notifications/appsettings.json /root/social_net_app/MiniSpace.Services.Notifications/src/MiniSpace.Services.Notifications.Api/
cp /tmp/events_public_settings/Services.Notifications/appsettings.docker.json /root/social_net_app/MiniSpace.Services.Notifications/src/MiniSpace.Services.Notifications.Api/

# cp /tmp/events_public_settings/Services.Organizations/appsettings.json /root/social_net_app/MiniSpace.Services.Organizations/src/MiniSpace.Services.Organizations.Api/
# cp /tmp/events_public_settings/Services.Organizations/appsettings.docker.json /root/social_net_app/MiniSpace.Services.Organizations/src/MiniSpace.Services.Organizations.Api/

cp /tmp/events_public_settings/Services.Posts/appsettings.json /root/social_net_app/MiniSpace.Services.Posts/src/MiniSpace.Services.Posts.Api/
cp /tmp/events_public_settings/Services.Posts/appsettings.docker.json /root/social_net_app/MiniSpace.Services.Posts/src/MiniSpace.Services.Posts.Api/

cp /tmp/events_public_settings/Services.Reactions/appsettings.json /root/social_net_app
