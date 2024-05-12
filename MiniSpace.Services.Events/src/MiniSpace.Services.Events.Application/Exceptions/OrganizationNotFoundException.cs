using System;

namespace MiniSpace.Services.Events.Application.Exceptions
{
    public class OrganizationNotFoundException: AppException
    {
        public override string Code { get; } = "organization_not_found";
        public Guid OrganizationId { get; }

        public OrganizationNotFoundException(Guid organizationId): base($"Organization with id: '{organizationId}' was not found.")
        {
            OrganizationId = organizationId;
        }
    }
}