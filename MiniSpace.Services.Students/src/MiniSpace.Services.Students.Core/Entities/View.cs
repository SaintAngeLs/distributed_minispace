using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class View
    {
        public Guid UserProfileId { get; private set; }
        public DateTime Date { get; private set; }

        public View(Guid postId, DateTime date)
        {
            UserProfileId = postId;
            Date = date;
        }
    }
}