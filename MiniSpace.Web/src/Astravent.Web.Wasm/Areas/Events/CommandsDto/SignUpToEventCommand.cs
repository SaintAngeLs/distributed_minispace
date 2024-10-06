using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Astravent.Web.Wasm.DTO.Enums;
using Astravent.Web.Wasm.DTO.Events;

namespace Astravent.Web.Wasm.Areas.Events.CommandsDto
{
    public class SignUpToEventCommand
    {
        public Guid EventId { get; set; }
        public Guid StudentId { get; set; }
    }
}
