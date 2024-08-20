using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSpace.Services.Comments.Core.Entities
{
    public enum CommentContext
    {
        UserPost,
        UserEvent,
        OrganizationPost,
        OrganizationEvent
    }
}
