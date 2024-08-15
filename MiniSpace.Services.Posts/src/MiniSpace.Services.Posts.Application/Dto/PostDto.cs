using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Posts.Core.Entities;

namespace MiniSpace.Services.Posts.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class PostDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid? EventId { get; set; }
        public string TextContent { get; set; }
        public IEnumerable<string> MediaFiles { get; set; }
        public string State { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Context { get; set; }
        public string Visibility { get; set; }

        public PostDto()
        {
        }

        public PostDto(Post post)
        {
            Id = post.Id;
            UserId = post.UserId;
            OrganizationId = post.OrganizationId;
            EventId = post.EventId;
            TextContent = post.TextContent;
            MediaFiles = post.MediaFiles;
            State = post.State.ToString();
            PublishDate = post.PublishDate;
            CreatedAt = post.CreatedAt;
            UpdatedAt = post.UpdatedAt;
            Context = post.Context.ToString();
            Visibility = post.Visibility.ToString(); 
        }
    }
}
