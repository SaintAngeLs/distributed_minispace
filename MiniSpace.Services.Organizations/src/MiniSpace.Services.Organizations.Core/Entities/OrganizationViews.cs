using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class OrganizationsViews
    {
        public Guid UserId { get; private set; }
        public IEnumerable<View> Views { get; private set; }

        public OrganizationsViews(Guid userProfileId, IEnumerable<View> views)
        {
            UserId = userProfileId;
            Views = views ?? new List<View>();
        }

        public void AddView(Guid organizationId, DateTime date, string ipAddress, string deviceType, string operatingSystem)
        {
            var viewList = new List<View>(Views)
            {
                new View(organizationId, date, ipAddress, deviceType, operatingSystem)
            };
            Views = viewList;
        }

        public void RemoveView(Guid organizationId)
        {
            var viewList = new List<View>(Views);
            var viewToRemove = viewList.Find(view => view.OrganizationId == organizationId);
            if (viewToRemove != null)
            {
                viewList.Remove(viewToRemove);
                Views = viewList;
            }
        }
    }
}
