using System;
using System.Collections.Generic;

namespace Astravent.Web.Wasm.DTO.Communication
{
    public class UserChatDto
    {
        public Guid UserId { get; set; }
        public List<ChatDto> Chats { get; set; }
    }
}
