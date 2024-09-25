using System;
using System.Collections.Generic;
using Astravent.Web.Wasm.DTO.Enums.Posts;

namespace Astravent.Web.Wasm.Areas.Posts.CommandsDto
{
    public class UpdatePostCommand
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
        public string Visibility { get; set; }
        public string PostType { get; set; }
        public string Title { get; set; }
        public Guid? PageOwnerId { get; set; }
        public string PageOwnerType { get; set; }
    }
}
