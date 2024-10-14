using System;
using System.Collections.Generic;

namespace Astravent.Web.Wasm.DTO
{
    public class InvitationModel
    {
        public List<Guid> SelectedFriendIds { get; set; } = new List<Guid>();
    }
}
