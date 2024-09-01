using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class OrganizationViews
    {
        public Guid OrganizationId { get; private set; }
        public IEnumerable<OrganizationView> Views { get; private set; }

        public OrganizationViews(Guid organizationId, IEnumerable<OrganizationView> views)
        {
            OrganizationId = organizationId;
            Views = views ?? new List<OrganizationView>();
        }

        public void AddView(Guid userId, DateTime date, string ipAddress, string deviceType, string operatingSystem)
        {
            var viewList = new List<OrganizationView>(Views)
            {
                new OrganizationView(userId, date, ipAddress, deviceType, operatingSystem)
            };
            Views = viewList;
        }

        public void RemoveView(Guid userId)
        {
            var viewList = new List<OrganizationView>(Views);
            var viewToRemove = viewList.Find(view => view.UserId == userId);
            if (viewToRemove != null)
            {
                viewList.Remove(viewToRemove);
                Views = viewList;
            }
        }
    }
}
