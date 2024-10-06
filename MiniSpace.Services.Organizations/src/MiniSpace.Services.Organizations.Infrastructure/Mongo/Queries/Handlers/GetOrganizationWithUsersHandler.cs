using Paralax.CQRS.Queries;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetOrganizationWithUsersHandler : IQueryHandler<GetOrganizationWithUsers, OrganizationUsersDto>
    {
        private readonly IMongoRepository<OrganizationDocument, Guid> _organizationRepository;

        public GetOrganizationWithUsersHandler(IMongoRepository<OrganizationDocument, Guid> organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<OrganizationUsersDto> HandleAsync(GetOrganizationWithUsers query, CancellationToken cancellationToken)
        {
            var organizationDocument = await _organizationRepository.GetAsync(o => o.Id == query.OrganizationId);
            if (organizationDocument == null)
            {
                return null;
            }

            var organization = organizationDocument.AsEntity();
            var users = organization.Users.ToList(); 

            return new OrganizationUsersDto(organization, users);
        }
    }
}
