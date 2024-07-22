#!/bin/bash

# Define repository URLs and directories
SOURCE_REPO_URL="https://gitlab.com/distributed-asp-net-core-blazor-social-app/events_apsettings_dev.git"
TARGET_REPO_URL="https://github.com/SaintAngeLs/distributed_minispace.git"
SOURCE_REPO_DIR="events_apsettings_dev"
TARGET_REPO_DIR="distributed_minispace"

# Define directories mapping from source to target
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

# Clone the source repository
git clone $SOURCE_REPO_URL
cd $SOURCE_REPO_DIR || exit 1

# Clone the target repository
git clone $TARGET_REPO_URL ../$TARGET_REPO_DIR
cd ../$TARGET_REPO_DIR || exit 1

# Copy appsettings files to the corresponding directories
for src_dir in "${!DIR_MAP[@]}"; do
  target_dir=${DIR_MAP[$src_dir]}
  mkdir -p "$target_dir"
  cp -v "../$SOURCE_REPO_DIR/$src_dir/appsettings*" "$target_dir/"
done

# Commit and push changes to the target repository
git add .
git commit -m "Update appsettings files from $SOURCE_REPO_URL"
git push

# Clean up
cd ..
rm -rf $SOURCE_REPO_DIR
rm -rf $TARGET_REPO_DIR

echo "Appsettings files have been updated successfully."
