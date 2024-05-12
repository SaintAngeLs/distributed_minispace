using System;

namespace MiniSpace.Web.Models.Posts
{
    public class UpdatePostModel
    {
        public Guid PostId { get; set; }
        public string TextContent { get; set; }
        public string MediaContent { get; set; }
    }
}
