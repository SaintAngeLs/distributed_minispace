using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Web.Areas.Friends.CommandsDto
{
    public class WithdrawFriendRequestDto
    {
        public Guid InviterId { get; set; }
        public Guid InviteeId { get; set; }
    }
}