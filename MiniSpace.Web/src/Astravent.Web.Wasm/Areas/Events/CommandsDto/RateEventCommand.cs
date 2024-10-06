using System;

namespace Astravent.Web.Wasm.Areas.Events.CommandsDto
{
    public class RateEventCommand
    {
        public Guid EventId { get; set; }
        public int Rating { get; set; }
        public Guid StudentId { get; set; }
    }
}
