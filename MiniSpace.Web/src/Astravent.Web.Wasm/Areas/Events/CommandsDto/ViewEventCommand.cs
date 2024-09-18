using System;

namespace Astravent.Web.Wasm.Areas.Events.CommandsDto
{
    public class ViewEventCommand 
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }

        public ViewEventCommand(Guid userId, Guid eventId)
        {
            UserId = userId;
            EventId = eventId;
        }
    }
}
