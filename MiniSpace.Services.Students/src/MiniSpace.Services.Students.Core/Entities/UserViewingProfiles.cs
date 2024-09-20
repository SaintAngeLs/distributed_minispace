using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class UserViewingProfiles
    {
        public Guid UserId { get; private set; }
        public IEnumerable<UserProfileView> ViewedProfiles { get; private set; }

        public UserViewingProfiles(Guid userId, IEnumerable<UserProfileView> viewedProfiles)
        {
            UserId = userId;
            ViewedProfiles = viewedProfiles ?? new List<UserProfileView>();
        }

        public void AddViewedProfile(Guid userProfileId, DateTime date, string ipAddress, string deviceType, string operatingSystem)
        {
            var viewedList = new List<UserProfileView>(ViewedProfiles)
            {
                new UserProfileView(userProfileId, date, ipAddress, deviceType, operatingSystem)
            };
            ViewedProfiles = viewedList;
        }

        public void RemoveViewedProfile(Guid userProfileId)
        {
            var viewedList = new List<UserProfileView>(ViewedProfiles);
            var profileToRemove = viewedList.Find(view => view.UserProfileId == userProfileId);
            if (profileToRemove != null)
            {
                viewedList.Remove(profileToRemove);
                ViewedProfiles = viewedList;
            }
        }
    }
}
