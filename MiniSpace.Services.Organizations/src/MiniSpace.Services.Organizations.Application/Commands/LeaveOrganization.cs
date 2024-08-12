using Convey.CQRS.Commands;
using System;

namespace MiniSpace.Services.Organizations.Application.Commands
{
    public class LeaveOrganization : ICommand
    {
        public Guid UserId { get; }
        public Guid OrganizationId { get; }

        public LeaveOrganization(Guid userId, Guid organizationId)
        {
            UserId = userId;
            OrganizationId = organizationId;
        }
    }
}
