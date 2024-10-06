using Paralax.CQRS.Commands;
using MiniSpace.Services.Posts.Core.Entities;
using System;

namespace MiniSpace.Services.Posts.Application.Commands
{
    public class ChangePostState : ICommand
    {
        public Guid PostId { get; }
        public Guid? UserId { get; }
        public Guid? OrganizationId { get; }
        public Guid? EventId { get; }
        public string State { get; }
        public DateTime? PublishDate { get; }
        public PostContext Context { get; }

        public ChangePostState(Guid postId, Guid? userId, Guid? organizationId, Guid? eventId, string state, DateTime? publishDate, PostContext context)
        {
            PostId = postId;
            UserId = userId;
            OrganizationId = organizationId;
            EventId = eventId;
            State = state;
            PublishDate = publishDate;
            Context = context;
        }
    }    
}
