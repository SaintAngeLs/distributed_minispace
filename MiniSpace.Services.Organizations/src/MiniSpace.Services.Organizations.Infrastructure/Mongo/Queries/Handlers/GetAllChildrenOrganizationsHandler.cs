using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetAllChildrenOrganizationsHandler : IQueryHandler<GetAllChildrenOrganizations, IEnumerable<Guid>>
    {
        private readonly IMongoRepository<OrganizationDocument, Guid> _repository;

        public GetAllChildrenOrganizationsHandler(IMongoRepository<OrganizationDocument, Guid> repository)
            => _repository = repository;

        public async Task<IEnumerable<Guid>> HandleAsync(GetAllChildrenOrganizations query, CancellationToken cancellationToken)
        {
            var root = await _repository.GetAsync(o => o.Id == query.RootId);
            var organization = root?.AsEntity().GetSubOrganization(query.OrganizationId);
            var result = new List<Guid>();
            if (organization != null)
            {
                result.AddRange(Organization.FindAllChildrenOrganizations(organization));
            }

            return result;
        }
    }
}