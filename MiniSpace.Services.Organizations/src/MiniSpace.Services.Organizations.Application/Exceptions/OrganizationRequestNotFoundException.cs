using System;

namespace MiniSpace.Services.Organizations.Application.Exceptions
{
    public class OrganizationRequestNotFoundException : AppException
    {
        public override string Code { get; } = "organization_request_not_found";
        public Guid RequestId { get; }

        public OrganizationRequestNotFoundException(Guid requestId)
            : base($"Organization request with ID: {requestId} was not found.")
        {
            RequestId = requestId;
        }
    }
}
