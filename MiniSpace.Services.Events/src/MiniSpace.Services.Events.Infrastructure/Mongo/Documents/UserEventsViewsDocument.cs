using System;
using System.Collections.Generic;
using System.Linq;
using Paralax.Types;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Documents
{
    public class UserEventsViewsDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<ViewDocument> Views { get; set; } = new List<ViewDocument>();

        public static UserEventsViewsDocument FromEntity(EventsViews eventsViews)
        {
            return new UserEventsViewsDocument
            {
                Id = Guid.NewGuid(), 
                UserId = eventsViews.UserId,
                Views = new List<ViewDocument>(eventsViews.Views.Select(ViewDocument.FromEntity))
            };
        }

        public EventsViews ToEntity()
        {
            return new EventsViews(UserId, Views.Select(view => view.ToEntity()));
        }
    }
}
