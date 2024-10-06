using Paralax.CQRS.Queries;
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
        private readonly IOrganizationMembersRepository _organizationMembersRepository;

        public GetUserOrganizationsHandler(
            IOrganizationRepository organizationRepository,
            IOrganizationMembersRepository organizationMembersRepository)
        {
            _organizationRepository = organizationRepository;
            _organizationMembersRepository = organizationMembersRepository;
        }

        public async Task<IEnumerable<UserOrganizationsDto>> HandleAsync(GetUserOrganizations query, CancellationToken cancellationToken)
        {
            var organizations = await _organizationRepository.GetOrganizationsByUserAsync(query.UserId);
            var organizationDtos = new List<UserOrganizationsDto>();

            foreach (var org in organizations)
            {
                var users = await _organizationMembersRepository.GetMembersAsync(org.Id);
                organizationDtos.Add(new UserOrganizationsDto(org, users));
            }

            return organizationDtos;
        }
    }
}
