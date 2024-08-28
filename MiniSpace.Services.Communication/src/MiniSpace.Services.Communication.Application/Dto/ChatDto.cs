using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Communication.Application.Dto
{
    public class ChatDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Guid> ParticipantIds { get; set; }
        public List<MessageDto> Messages { get; set; }
    }
}