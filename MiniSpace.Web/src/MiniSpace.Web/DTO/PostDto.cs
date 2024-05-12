using System;

namespace MiniSpace.Web.DTO
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid OrganizerId { get; set; }
        public string TextContent { get; set; }
        public string MediaContent { get; set; }
        public string State { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}