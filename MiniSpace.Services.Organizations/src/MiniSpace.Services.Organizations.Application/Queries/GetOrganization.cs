using Paralax.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetOrganization : IQuery<OrganizationDto>
    {
        public Guid OrganizationId { get; set; }
    }
}