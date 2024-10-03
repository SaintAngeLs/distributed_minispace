using System;
using System.Collections.Generic;

namespace Astravent.Web.Wasm.Models.Posts
{
    public class UpdatePostModel
    {
        public Guid PostId { get; set; }
        public string TextContent { get; set; }
        public IEnumerable<Guid> MediaFiles { get; set; }
    }
}
