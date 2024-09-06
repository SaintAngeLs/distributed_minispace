using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class UserOrganizationsViews
    {
        public Guid UserId { get; private set; }
        public IEnumerable<UserView> Views { get; private set; }

        public UserOrganizationsViews(Guid userProfileId, IEnumerable<UserView> views)
        {
            UserId = userProfileId;
            Views = views ?? new List<UserView>();
        }

        public void AddView(Guid organizationId, DateTime date, string ipAddress, string deviceType, string operatingSystem)
        {
            var viewList = new List<UserView>(Views)
            {
                new UserView(organizationId, date, ipAddress, deviceType, operatingSystem)
            };
            Views = viewList;
        }

        public void RemoveView(Guid organizationId)
        {
            var viewList = new List<UserView>(Views);
            var viewToRemove = viewList.Find(view => view.OrganizationId == organizationId);
            if (viewToRemove != null)
            {
                viewList.Remove(viewToRemove);
                Views = viewList;
            }
        }
    }
}
