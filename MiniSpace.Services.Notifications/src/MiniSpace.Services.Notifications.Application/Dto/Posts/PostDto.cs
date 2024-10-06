using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Notifications.Application.Dto.Posts
{
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
    }
}