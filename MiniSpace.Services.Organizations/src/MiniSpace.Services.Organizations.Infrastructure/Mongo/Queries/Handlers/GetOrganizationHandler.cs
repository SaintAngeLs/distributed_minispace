using Paralax.CQRS.Queries;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Core.Repositories;
using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Organizations.Core.Entities; // Ensure this is imported
using System;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetOrganizationHandler : IQueryHandler<GetOrganization, OrganizationDto>
    {
        private readonly IOrganizationRepository _organizationRepository;

        public GetOrganizationHandler(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<OrganizationDto> HandleAsync(GetOrganization query, CancellationToken cancellationToken)
        {
            var organization = await _organizationRepository.GetAsync(query.OrganizationId);

            if (organization == null)
            {
                return null;
            }

            return new OrganizationDto(organization);
        }
    }
}
