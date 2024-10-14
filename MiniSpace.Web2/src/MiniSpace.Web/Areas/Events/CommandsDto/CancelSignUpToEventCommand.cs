using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MiniSpace.Web.DTO.Enums;
using MiniSpace.Web.DTO.Events;

namespace MiniSpace.Web.Areas.Events.CommandsDto
{
     public class CancelInterestInEventCommand
    {
        public Guid EventId { get; set; }
        public Guid StudentId { get; set; }
    }
}
