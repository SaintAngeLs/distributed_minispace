using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class UserProfileViewsForUser
    {
        public Guid UserId { get; private set; }
        public IEnumerable<UserProfileView> Views { get; private set; }

        public UserProfileViewsForUser(Guid userId, IEnumerable<UserProfileView> views)
        {
            UserId = userId;
            Views = views ?? new List<UserProfileView>();
        }

        public void AddView(Guid userProfileId, DateTime date, string ipAddress, string deviceType, string operatingSystem)
        {
            var viewList = new List<UserProfileView>(Views)
            {
                new UserProfileView(userProfileId, date, ipAddress, deviceType, operatingSystem)
            };
            Views = viewList;
        }

        public void RemoveView(Guid userProfileId)
        {
            var viewList = new List<UserProfileView>(Views);
            var viewToRemove = viewList.Find(view => view.UserProfileId == userProfileId);
            if (viewToRemove != null)
            {
                viewList.Remove(viewToRemove);
                Views = viewList;
            }
        }
    }
}
