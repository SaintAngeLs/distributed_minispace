using System;
using System.Collections.Generic;

namespace MiniSpace.Web.DTO
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid OrganizerId { get; set; }
        public string TextContent { get; set; }
        public IEnumerable<Guid> MediaFiles { get; set; }
        public string State { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}