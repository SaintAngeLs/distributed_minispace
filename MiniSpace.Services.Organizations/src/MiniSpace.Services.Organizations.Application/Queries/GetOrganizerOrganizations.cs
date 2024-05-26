using Convey.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using System.Diagnostics.CodeAnalysis;


namespace MiniSpace.Services.Organizations.Application.Queries
{
    [ExcludeFromCodeCoverage]

    public class GetOrganizerOrganizations: IQuery<IEnumerable<OrganizationDto>>
    {
        
        public Guid OrganizerId { get; set; }
        public Guid RootId { get; set; }
    }
}

