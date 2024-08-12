using Convey.CQRS.Commands;
using System;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class RejectFollowRequest : ICommand
    {
        public Guid OrganizationId { get; set; }
        public Guid RequestId { get; set; }
        public string Reason { get; set; }

        public RejectFollowRequest(Guid organizationId, Guid requestId, string reason)
        {
            OrganizationId = organizationId;
            RequestId = requestId;
            Reason = reason;
        }
    }
}
