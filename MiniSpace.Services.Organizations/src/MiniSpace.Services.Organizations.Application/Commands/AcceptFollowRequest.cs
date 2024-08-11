using Convey.CQRS.Commands;
using System;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class AcceptFollowRequest : ICommand
    {
        public Guid OrganizationId { get; }
        public Guid RequestId { get; }

        public AcceptFollowRequest(Guid organizationId, Guid requestId)
        {
            OrganizationId = organizationId;
            RequestId = requestId;
        }
    }
}
