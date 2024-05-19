using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;
using MongoDB.Driver;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    public class GetOrganizationDetailsHandler : IQueryHandler<GetOrganizationDetails, OrganizationDetailsDto>
    {
        private readonly IMongoRepository<OrganizationDocument, Guid> _repository;

        public GetOrganizationDetailsHandler(IMongoRepository<OrganizationDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<OrganizationDetailsDto> HandleAsync(GetOrganizationDetails query, CancellationToken cancellationToken)
        {
            var root = await _repository.GetAsync(o => o.Id == query.RootId);
            var organization = root?.AsEntity().GetSubOrganization(query.OrganizationId);
            return organization == null ? null : new OrganizationDetailsDto(organization, root.Id);
        }
    }
}