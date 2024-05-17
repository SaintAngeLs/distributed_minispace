using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    public class GetOrganizationHandler : IQueryHandler<GetOrganization, OrganizationDto>
    {
        private readonly IMongoRepository<OrganizationDocument, Guid> _repository;

        public GetOrganizationHandler(IMongoRepository<OrganizationDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<OrganizationDto> HandleAsync(GetOrganization query, CancellationToken cancellationToken)
        {
            var root = await _repository.GetAsync(o => o.Id == query.RootId);
            var organization = root?.AsEntity().GetSubOrganization(query.OrganizationId);
            return organization == null ? null : new OrganizationDto(organization);
        }
    }
}