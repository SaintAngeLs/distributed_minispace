using System;
using Astravent.Web.Wasm.DTO.Enums.Posts;

namespace Astravent.Web.Wasm.Areas.Posts.CommandsDto
{
    public class RepostCommand
    {
        public Guid OriginalPostId { get; set; }
        public Guid RepostedPostId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid? EventId { get; set; }
        public PostContext Context { get; set; }
        public Guid? PageOwnerId { get; set; }
        public string PageOwnerType { get; set; }

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
