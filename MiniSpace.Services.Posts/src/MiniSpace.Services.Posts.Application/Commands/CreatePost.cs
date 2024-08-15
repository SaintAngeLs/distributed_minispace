using Convey.CQRS.Commands;
using MiniSpace.Services.Posts.Core.Entities;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Posts.Application.Commands
{
    public class CreatePost : ICommand
    {
        public Guid PostId { get; }
        public Guid? UserId { get; }
        public Guid? OrganizationId { get; }
        public Guid? EventId { get; }
        public string TextContent { get; }
        public IEnumerable<string> MediaFiles { get; }
        public string State { get; }
        public DateTime? PublishDate { get; }
        public PostContext Context { get; }
        public string Visibility { get; set; }  

        public CreatePost(Guid postId, Guid? userId, Guid? organizationId, Guid? eventId, string textContent,
            IEnumerable<string> mediaFiles, string state, DateTime? publishDate, PostContext context, string visibility)
        {
            PostId = postId;
            UserId = userId;
            OrganizationId = organizationId;
            EventId = eventId;
            TextContent = textContent;
            MediaFiles = mediaFiles;
            State = state;
            PublishDate = publishDate;
            Context = context;
            Visibility = visibility; 
        }
    }
}
