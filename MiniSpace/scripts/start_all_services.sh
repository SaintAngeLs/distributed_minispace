#!/bin/bash

# Determine the appropriate terminal command based on the desktop environment
case "$XDG_CURRENT_DESKTOP" in
    GNOME)
        TERMINAL_CMD="gnome-terminal --tab -- bash -c"
        ;;
    XFCE)
        TERMINAL_CMD="xfce4-terminal --tab -x bash -c"
        ;;
    KDE)
        TERMINAL_CMD="konsole --new-tab -e bash -c"
        ;;
    *)
        # Default to x-terminal-emulator if no known desktop environment is found
        TERMINAL_CMD="x-terminal-emulator --tab -e bash -c"
        ;;
esac

# Define the root path relative to the script location
ROOT_PATH="../../"

# Define a list of service directories
SERVICES=(
    "MiniSpace.APIGateway/src/MiniSpace.APIGateway"
    "MiniSpace.Services.Comments/src/MiniSpace.Services.Comments.Api"
    "MiniSpace.Services.Email/src/MiniSpace.Services.Email.Api"
    "MiniSpace.Services.Events/src/MiniSpace.Services.Events.Api"
    "MiniSpace.Services.Friends/src/MiniSpace.Services.Friends.Api"
    "MiniSpace.Services.Identity/src/MiniSpace.Services.Identity.Api"
    "MiniSpace.Services.MediaFiles/src/MiniSpace.Services.MediaFiles.Api"
    "MiniSpace.Services.Notifications/src/MiniSpace.Services.Notifications.Api"
    "MiniSpace.Services.Organizations/src/MiniSpace.Services.Organizations.Api"
    "MiniSpace.Services.Posts/src/MiniSpace.Services.Posts.Api"
    "MiniSpace.Services.Reactions/src/MiniSpace.Services.Reactions.Api"
    "MiniSpace.Services.Reports/src/MiniSpace.Services.Reports.Api"
    "MiniSpace.Services.Students/src/MiniSpace.Services.Students.Api"
)

echo "Starting all MiniSpace services..."

# Iterate over each service and open it in a new terminal tab
for SERVICE in "${SERVICES[@]}"
do
    FULL_PATH="${ROOT_PATH}${SERVICE}"
    # Construct the command to run in the new tab
    RUN_CMD="echo Starting service in $(pwd); cd '$FULL_PATH' && dotnet run; exec bash"
    FINAL_CMD="$TERMINAL_CMD \"$RUN_CMD\""
    eval $FINAL_CMD &
    if [ $? -ne 0 ]; then
        echo "Failed to start $SERVICE"
    fi
done

echo "All services started or attempted to start in new terminal tabs."
