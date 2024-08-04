using Convey.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetOrganizationRolesHandler : IQueryHandler<GetOrganizationRoles, IEnumerable<RoleDto>>
    {
        private readonly IOrganizationRolesRepository _organizationRolesRepository;

        public GetOrganizationRolesHandler(IOrganizationRolesRepository organizationRolesRepository)
        {
            _organizationRolesRepository = organizationRolesRepository;
        }

        public async Task<IEnumerable<RoleDto>> HandleAsync(GetOrganizationRoles query, CancellationToken cancellationToken)
        {
            var roles = await _organizationRolesRepository.GetRolesAsync(query.OrganizationId);
            return roles?.Select(role => new RoleDto(role)) ?? Enumerable.Empty<RoleDto>();
        }
    }
}
