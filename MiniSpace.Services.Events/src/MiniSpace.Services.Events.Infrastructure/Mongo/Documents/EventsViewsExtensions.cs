using System;
using System.Linq;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Documents
{
    public static class EventsViewsExtensions
    {
        public static UserEventsViewsDto AsDto(this UserEventsViewsDocument document)
        {
            return new UserEventsViewsDto
            {
                UserId = document.UserId,
                Views = document.Views.Select(v => new ViewDto
                {
                    EventId = v.EventId,
                    Date = v.Date
                })
            };
        }

        public static UserEventsViewsDocument AsDocument(this EventsViews entity)
        {
            return new UserEventsViewsDocument
            {
                Id = Guid.NewGuid(),
                UserId = entity.UserId,
                Views = entity.Views.Select(ViewDocument.FromEntity).ToList()
            };
        }

        public static EventsViews AsEntity(this UserEventsViewsDocument document)
        {
            return new EventsViews(
                document.UserId,
                document.Views.Select(v => new View(v.EventId, v.Date))
            );
        }
    }
}
