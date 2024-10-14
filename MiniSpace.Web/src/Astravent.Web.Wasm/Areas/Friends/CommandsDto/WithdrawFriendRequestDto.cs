using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astravent.Web.Wasm.Areas.Friends.CommandsDto
{
    public class WithdrawFriendRequestDto
    {
        public Guid InviterId { get; set; }
        public Guid InviteeId { get; set; }
    }
}