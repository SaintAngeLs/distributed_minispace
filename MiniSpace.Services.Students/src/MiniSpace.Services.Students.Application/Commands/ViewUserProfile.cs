using System;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class ViewUserProfile : ICommand
    {
        public Guid UserId { get; }
        public Guid UserProfileId { get; }

        public ViewUserProfile(Guid userId, Guid userProfileId)
        {
            UserId = userId;
            UserProfileId = userProfileId;
        }
    }
}
