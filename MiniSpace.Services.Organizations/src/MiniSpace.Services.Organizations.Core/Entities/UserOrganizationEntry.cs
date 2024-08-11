using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class UserOrganizationEntry
    {
        public Guid OrganizationId { get; }
        public DateTime JoinDate { get; }

        public UserOrganizationEntry(Guid organizationId, DateTime joinDate)
        {
            OrganizationId = organizationId;
            JoinDate = joinDate;
        }
    }
}