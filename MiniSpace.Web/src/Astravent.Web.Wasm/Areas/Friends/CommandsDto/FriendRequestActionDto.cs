using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astravent.Web.Wasm.Areas.Friends.CommandsDto
{
    public class FriendRequestActionDto
    {
        public Guid RequestId { get; set; }
        public Guid RequesterId { get; set; }
        public Guid FriendId { get; set; }
    }
}