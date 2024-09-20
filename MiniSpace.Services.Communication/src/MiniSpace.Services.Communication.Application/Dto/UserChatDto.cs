using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Communication.Application.Dto
{
    public class UserChatDto
    {
        public Guid UserId { get; set; }
        public List<ChatDto> Chats { get; set; }
    }
}
