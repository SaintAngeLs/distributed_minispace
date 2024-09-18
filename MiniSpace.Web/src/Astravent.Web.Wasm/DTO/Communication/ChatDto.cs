using System;
using System.Collections.Generic;

namespace Astravent.Web.Wasm.DTO.Communication
{
    public class ChatDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Guid> ParticipantIds { get; set; }
        public List<MessageDto> Messages { get; set; }
    }
}
