using Convey.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;

namespace MiniSpace.Services.Organizations.Application.Queries
{
    public class GetChildrenOrganizations: IQuery<IEnumerable<OrganizationDto>>
    {
        public Guid ParentId { get; set; }
    }
}