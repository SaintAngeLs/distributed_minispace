using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Web.Areas.Students.CommandsDto
{
    public class ViewUserProfileCommand
    {
        public Guid UserId { get; }
        public Guid UserProfileId { get; }

        public ViewUserProfileCommand(Guid userId, Guid userProfileId)
        {
            UserId = userId;
            UserProfileId = userProfileId;
        }
    }
}