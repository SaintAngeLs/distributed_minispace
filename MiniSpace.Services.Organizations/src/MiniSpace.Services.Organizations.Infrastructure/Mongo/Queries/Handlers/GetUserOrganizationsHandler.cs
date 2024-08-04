using Convey.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    public class GetUserOrganizationsHandler : IQueryHandler<GetUserOrganizations, IEnumerable<UserOrganizationsDto>>
    {
        private readonly IOrganizationRepository _organizationRepository;

        public GetUserOrganizationsHandler(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<IEnumerable<UserOrganizationsDto>> HandleAsync(GetUserOrganizations query, CancellationToken cancellationToken)
        {
            var organizations = await _organizationRepository.GetOrganizationsByUserAsync(query.UserId);
            return organizations.Select(org => new UserOrganizationsDto(org));
        }
    }
}
