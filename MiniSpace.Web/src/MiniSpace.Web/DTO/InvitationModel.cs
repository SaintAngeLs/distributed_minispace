using System;
using System.Collections.Generic;

namespace MiniSpace.Web.DTO
{
    public class InvitationModel
    {
        public List<Guid> SelectedFriendIds { get; set; } = new List<Guid>();
    }
}
