using Convey.CQRS.Commands;
using MiniSpace.Services.Posts.Core.Entities;
using System;

namespace MiniSpace.Services.Posts.Application.Commands
{
    public class RepostCommand : ICommand
    {
        public Guid OriginalPostId { get; }
        public Guid RepostedPostId { get; }
        public Guid? UserId { get; }
        public Guid? OrganizationId { get; }
        public Guid? EventId { get; }
        public PostContext Context { get; }
        public Guid? PageOwnerId { get; }
        public string PageOwnerType { get; }

        public RepostCommand(
            Guid originalPostId,
            Guid repostedPostId,
            Guid? userId, 
            Guid? organizationId, 
            Guid? eventId,
            PostContext context,
            Guid? pageOwnerId = null,
            string pageOwnerType = "User")
        {
            OriginalPostId = originalPostId;
            RepostedPostId = repostedPostId;
            UserId = userId;
            OrganizationId = organizationId;
            EventId = eventId;
            Context = context;
            PageOwnerId = pageOwnerId;
            PageOwnerType = pageOwnerType;
        }
    }
}
