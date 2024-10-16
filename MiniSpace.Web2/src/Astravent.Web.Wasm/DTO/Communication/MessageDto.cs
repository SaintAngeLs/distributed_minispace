using System;
using System.Collections.Generic;

namespace Astravent.Web.Wasm.DTO.Communication
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; }
        public Guid ChatId { get; set; }
        public DateTime Timestamp { get; set; }
        public string MessageType { get; set; }
        public string Status { get; set; }
    }

}
