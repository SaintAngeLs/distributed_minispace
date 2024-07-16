using System;
using System.Collections.Generic;

namespace MiniSpacePwa.Models.Posts
{
    public class UpdatePostModel
    {
        public Guid PostId { get; set; }
        public string TextContent { get; set; }
        public IEnumerable<Guid> MediaFiles { get; set; }
    }
}
