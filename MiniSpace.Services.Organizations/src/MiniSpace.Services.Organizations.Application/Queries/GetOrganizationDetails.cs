using Paralax.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetOrganizationDetails : IQuery<OrganizationDetailsDto>
    {
        public Guid OrganizationId { get; set; }
    }
}