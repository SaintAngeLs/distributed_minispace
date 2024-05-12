using System;

namespace MiniSpace.Web.Models.Posts
{
    public class CreatePostModel
    {
        public Guid PostId { get; set; }
        public Guid EventId { get; set; }
        public Guid OrganizerId { get; set; }
        public string TextContent { get; set; }
        public string MediaContent { get; set; }
        public string State { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
