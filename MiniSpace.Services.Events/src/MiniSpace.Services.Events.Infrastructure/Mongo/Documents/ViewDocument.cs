using System;
using Paralax.Types;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Documents
{
    public class ViewDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public DateTime Date { get; set; }

        public static ViewDocument FromEntity(View view)
        {
            return new ViewDocument
            {
                Id = Guid.NewGuid(),
                EventId = view.EventId,
                Date = view.Date
            };
        }

        public View ToEntity()
        {
            return new View(EventId, Date);
        }
    }
}
