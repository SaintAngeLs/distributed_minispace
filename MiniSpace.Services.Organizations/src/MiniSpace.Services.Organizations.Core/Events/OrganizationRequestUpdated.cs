using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class OrganizationRequestUpdated : IDomainEvent
    {
        public Guid RequestId { get; }
        public Guid OrganizationId { get; }
        public Guid UserId { get; }
        public DateTime UpdateDate { get; }
        public string NewReason { get; }

        public OrganizationRequestUpdated(Guid requestId, Guid organizationId, Guid userId, DateTime updateDate, string newReason)
        {
            RequestId = requestId;
            OrganizationId = organizationId;
            UserId = userId;
            UpdateDate = updateDate;
            NewReason = newReason;
        }
    }
}