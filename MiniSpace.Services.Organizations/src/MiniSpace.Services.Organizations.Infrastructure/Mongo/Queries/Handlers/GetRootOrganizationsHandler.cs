using Paralax.CQRS.Queries;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetRootOrganizationsHandler : IQueryHandler<GetRootOrganizations, IEnumerable<OrganizationDto>>
    {
        private readonly IMongoRepository<OrganizationDocument, Guid> _repository;

        public GetRootOrganizationsHandler(IMongoRepository<OrganizationDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<OrganizationDto>> HandleAsync(GetRootOrganizations query, CancellationToken cancellationToken)
        {
            var rootOrganizations = await _repository.FindAsync(o => o.ParentOrganizationId == null);
            return rootOrganizations.Select(o => new OrganizationDto(o.AsEntity()));
        }
    }
}
