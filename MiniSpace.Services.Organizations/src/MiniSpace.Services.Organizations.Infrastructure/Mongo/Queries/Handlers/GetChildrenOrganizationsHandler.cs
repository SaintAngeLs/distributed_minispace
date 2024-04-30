using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    public class GetChildrenOrganizationsHandler : IQueryHandler<GetChildrenOrganizations, IEnumerable<OrganizationDto>>
    {
        private readonly IMongoRepository<OrganizationDocument, Guid> _repository;

        public GetChildrenOrganizationsHandler(IMongoRepository<OrganizationDocument, Guid> repository)
            => _repository = repository;

        public async Task<IEnumerable<OrganizationDto>> HandleAsync(GetChildrenOrganizations query, CancellationToken cancellationToken)
        {
            var organizations = await _repository.FindAsync(o => o.ParentId == query.ParentId);

            return organizations.Select(o => o.AsDto());
        }
    }
}