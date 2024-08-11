using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Core.Events
{
    public class OrganizationRequestCanceled : IDomainEvent
    {
        public Guid RequestId { get; }
        public Guid OrganizationId { get; }
        public Guid UserId { get; }
        public DateTime CancellationDate { get; }

        public OrganizationRequestCanceled(Guid requestId, Guid organizationId, Guid userId, DateTime cancellationDate)
        {
            RequestId = requestId;
            OrganizationId = organizationId;
            UserId = userId;
            CancellationDate = cancellationDate;
        }
    }
}