using System;
using System.Collections.Generic;
using MiniSpace.Web.DTO.Enums.Posts;

namespace MiniSpace.Web.Areas.Posts.CommandsDto
{
    public class CreatePostCommand
    {
        public Guid PostId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid? EventId { get; set; }
        public string TextContent { get; set; }
        public IEnumerable<string> MediaFiles { get; set; }
        public string State { get; set; }
        public DateTime? PublishDate { get; set; }
        public PostContext Context { get; set; }
    }
}
