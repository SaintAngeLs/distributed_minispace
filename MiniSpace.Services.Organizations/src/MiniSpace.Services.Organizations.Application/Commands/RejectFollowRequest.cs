using Convey.CQRS.Commands;
using System;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class RejectFollowRequest : ICommand
    {
        public Guid OrganizationId { get; }
        public Guid RequestId { get; }
        public string Reason { get; }

        public RejectFollowRequest(Guid organizationId, Guid requestId, string reason)
        {
            OrganizationId = organizationId;
            RequestId = requestId;
            Reason = reason;
        }
    }
}
