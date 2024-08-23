using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class OrganizationRequestApproved : IDomainEvent
    {
        public Guid RequestId { get; }
        public Guid OrganizationId { get; }
        public Guid UserId { get; }
        public DateTime ApprovalDate { get; }

        public OrganizationRequestApproved(Guid requestId, Guid organizationId, Guid userId, DateTime approvalDate)
        {
            RequestId = requestId;
            OrganizationId = organizationId;
            UserId = userId;
            ApprovalDate = approvalDate;
        }
    }
}