#!/bin/bash


SOURCE_REPO_URL="https://gitlab.com/distributed-asp-net-core-blazor-social-app/events_apsettings_dev.git"
TARGET_REPO_DIR="../../.."
SOURCE_REPO_DIR="./events_apsettings_dev"

declare -A DIR_MAP
DIR_MAP["APIGateway"]="MiniSpace.APIGateway/src/MiniSpace.APIGateway"
DIR_MAP["Services.Comments"]="MiniSpace.Services.Comments/src/MiniSpace.Services.Comments.Api"
DIR_MAP["Services.Email"]="MiniSpace.Services.Email/src/MiniSpace.Services.Email.Api"
DIR_MAP["Services.Events"]="MiniSpace.Services.Events/src/MiniSpace.Services.Events.Api"
DIR_MAP["Services.Friends"]="MiniSpace.Services.Friends/src/MiniSpace.Services.Friends.Api"
DIR_MAP["Services.Identity"]="MiniSpace.Services.Identity/src/MiniSpace.Services.Identity.Api"
DIR_MAP["Services.MediaFiles"]="MiniSpace.Services.MediaFiles/src/MiniSpace.Services.MediaFiles.Api"
DIR_MAP["Services.Notifications"]="MiniSpace.Services.Notifications/src/MiniSpace.Services.Notifications.Api"
DIR_MAP["Services.Organizations"]="MiniSpace.Services.Organizations/src/MiniSpace.Services.Organizations.Api"
DIR_MAP["Services.Posts"]="MiniSpace.Services.Posts/src/MiniSpace.Services.Posts.Api"
DIR_MAP["Services.Reactions"]="MiniSpace.Services.Reactions/src/MiniSpace.Services.Reactions.Api"
DIR_MAP["Services.Reports"]="MiniSpace.Services.Reports/src/MiniSpace.Services.Reports.Api"
DIR_MAP["Services.Students"]="MiniSpace.Services.Students/src/MiniSpace.Services.Students.Api"
DIR_MAP["Web"]="MiniSpace.Web/src/MiniSpace.Web"


git clone $SOURCE_REPO_URL $SOURCE_REPO_DIR
cd $SOURCE_REPO_DIR || exit 1

for src_dir in "${!DIR_MAP[@]}"; do
  target_dir=$TARGET_REPO_DIR/${DIR_MAP[$src_dir]}
  if [ -d "$target_dir" ]; then
    mkdir -p "$target_dir"
    cp -v "$src_dir/appsettings"* "$target_dir/"
  else
    ls -la
    echo "Target directory $target_dir does not exist. Skipping..."
  fi
done


cd ..
rm -rf $SOURCE_REPO_DIR

echo "Appsettings files have been updated successfully."
