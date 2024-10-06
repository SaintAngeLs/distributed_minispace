using Paralax.CQRS.Commands;
using MiniSpace.Services.Posts.Core.Entities;
using System;

namespace MiniSpace.Services.Posts.Application.Commands
{
    public class DeletePost : ICommand
    {
        public Guid PostId { get; }
        public Guid? UserId { get; }
        public Guid? OrganizationId { get; }
        public Guid? EventId { get; }
        public string Context { get; }

        public DeletePost(Guid postId, Guid? userId, Guid? organizationId, Guid? eventId, string context)
        {
            PostId = postId;
            UserId = userId;
            OrganizationId = organizationId;
            EventId = eventId;
            Context = context;
        }
    }
}
