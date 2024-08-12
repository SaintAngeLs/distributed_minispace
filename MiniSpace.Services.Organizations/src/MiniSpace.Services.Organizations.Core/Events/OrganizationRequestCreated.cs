using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class OrganizationRequestCreated : IDomainEvent
    {
        public Guid RequestId { get; }
        public Guid OrganizationId { get; }
        public Guid UserId { get; }
        public DateTime RequestDate { get; }

        public OrganizationRequestCreated(Guid requestId, Guid organizationId, Guid userId, DateTime requestDate)
        {
            RequestId = requestId;
            OrganizationId = organizationId;
            UserId = userId;
            RequestDate = requestDate;
        }
    }
}