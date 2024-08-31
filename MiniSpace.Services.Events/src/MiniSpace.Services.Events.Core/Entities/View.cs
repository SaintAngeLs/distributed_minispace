using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class View
    {
        public Guid EventId { get; private set; }
        public DateTime Date { get; private set; }

        public View(Guid eventId, DateTime date)
        {
            EventId = eventId;
            Date = date;
        }
    }
}