using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Posts.Core.Entities
{
    public class View
    {
        public Guid PostId { get; private set; }
        public DateTime Date { get; private set; }

        public View(Guid postId, DateTime date)
        {
            PostId = postId;
            Date = date;
        }
    }
}