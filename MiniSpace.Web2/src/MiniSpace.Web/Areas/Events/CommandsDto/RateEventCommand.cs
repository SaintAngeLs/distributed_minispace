using System;

namespace MiniSpace.Web.Areas.Events.CommandsDto
{
    public class RateEventCommand
    {
        public Guid EventId { get; set; }
        public int Rating { get; set; }
        public Guid StudentId { get; set; }
    }
}
