using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class OrganizationRequestRejected : IDomainEvent
    {
        public Guid RequestId { get; }
        public Guid OrganizationId { get; }
        public Guid UserId { get; }
        public DateTime RejectionDate { get; }
        public string RejectionReason { get; }

        public OrganizationRequestRejected(Guid requestId, Guid organizationId, Guid userId, DateTime rejectionDate, string rejectionReason)
        {
            RequestId = requestId;
            OrganizationId = organizationId;
            UserId = userId;
            RejectionDate = rejectionDate;
            RejectionReason = rejectionReason;
        }
    }
}