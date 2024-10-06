using Paralax.CQRS.Commands;
using System;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class AcceptFollowRequest : ICommand
    {
        public Guid OrganizationId { get; set; }
        public Guid RequestId { get; set; }

        public AcceptFollowRequest(Guid organizationId, Guid requestId)
        {
            OrganizationId = organizationId;
            RequestId = requestId;
        }
    }
}
