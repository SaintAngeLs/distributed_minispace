using System;
using System.Collections.Generic;

namespace MiniSpace.Web.DTO.Communication
{
    public class MessageStatusUpdateDto
    {
        public string ChatId { get; set; }
        public string MessageId { get; set; }
        public string Status { get; set; }
    }

}
