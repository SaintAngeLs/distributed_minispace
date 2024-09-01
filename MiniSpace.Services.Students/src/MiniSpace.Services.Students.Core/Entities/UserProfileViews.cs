using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class UserProfileViews
    {
        public Guid UserId { get; private set; }
        public IEnumerable<View> Views { get; private set; }

        public UserProfileViews(Guid userProfileId, IEnumerable<View> views)
        {
            UserId = userProfileId;
            Views = views ?? new List<View>();
        }

        public void AddView(Guid userProfileId, DateTime date)
        {
            var viewList = new List<View>(Views)
            {
                new View(userProfileId, date)
            };
            Views = viewList;
        }

        public void RemoveView(Guid userProfileId)
        {
            var viewList = new List<View>(Views);
            var viewToRemove = viewList.Find(view => view.UserProfileId == userProfileId);
            if (viewToRemove != null)
            {
                viewList.Remove(viewToRemove);
                Views = viewList;
            }
        }
    }
}
