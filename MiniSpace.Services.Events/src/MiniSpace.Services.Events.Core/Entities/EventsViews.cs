using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Events.Core.Entities
{
    public class EventsViews
    {
        public Guid UserId { get; private set; }
        public IEnumerable<View> Views { get; private set; }

        public EventsViews(Guid userId, IEnumerable<View> views)
        {
            UserId = userId;
            Views = views ?? new List<View>();
        }

        public void AddView(Guid eventId, DateTime date)
        {
            var viewList = new List<View>(Views)
            {
                new View(eventId, date)
            };
            Views = viewList;
        }

        public void RemoveView(Guid eventId)
        {
            var viewList = new List<View>(Views);
            var viewToRemove = viewList.Find(view => view.EventId == eventId);
            if (viewToRemove != null)
            {
                viewList.Remove(viewToRemove);
                Views = viewList;
            }
        }
    }
}
