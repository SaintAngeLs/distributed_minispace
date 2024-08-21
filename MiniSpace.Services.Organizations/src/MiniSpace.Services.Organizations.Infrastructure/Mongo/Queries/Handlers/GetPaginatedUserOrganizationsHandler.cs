using Convey.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Core.Repositories;
using MiniSpace.Services.Organizations.Application.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    public class GetPaginatedUserOrganizationsHandler : IQueryHandler<GetPaginatedUserOrganizations, MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOrganizationMembersRepository _organizationMembersRepository;

        public GetPaginatedUserOrganizationsHandler(
            IOrganizationRepository organizationRepository,
            IOrganizationMembersRepository organizationMembersRepository)
        {
            _organizationRepository = organizationRepository;
            _organizationMembersRepository = organizationMembersRepository;
        }

        public async Task<MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>> HandleAsync(GetPaginatedUserOrganizations query, CancellationToken cancellationToken)
        {
            // Fetch organizations where the user is a member
            var organizations = await _organizationRepository.GetOrganizationsByUserAsync(query.UserId);
            var organizationDtos = new List<OrganizationDto>();

            foreach (var org in organizations)
            {
                var users = await _organizationMembersRepository.GetMembersAsync(org.Id);
                var organizationDto = new OrganizationDto
                {
                    Id = org.Id,
                    Name = org.Name,
                    Description = org.Description,
                    ImageUrl = org.ImageUrl,
                    BannerUrl = org.BannerUrl,
                    Users = users.Select(u => new UserDto(u)).ToList(),
                };
                organizationDtos.Add(organizationDto);
            }

            // Paginate the result
            var totalItems = organizationDtos.Count;
            var paginatedOrganizations = organizationDtos
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            return new MiniSpace.Services.Organizations.Application.DTO.PagedResult<OrganizationDto>(paginatedOrganizations, query.Page, query.PageSize, totalItems);
        }
    }
}
