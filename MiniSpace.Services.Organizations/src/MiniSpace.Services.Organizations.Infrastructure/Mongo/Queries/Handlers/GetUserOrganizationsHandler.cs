using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    public class GetUserOrganizationsHandler : IQueryHandler<GetUserOrganizations, IEnumerable<OrganizationDto>>
    {
        private readonly IMongoRepository<OrganizationDocument, Guid> _repository;

        public GetUserOrganizationsHandler(IMongoRepository<OrganizationDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<OrganizationDto>> HandleAsync(GetUserOrganizations query, CancellationToken cancellationToken)
        {
            var filter = Builders<OrganizationDocument>.Filter.Eq(o => o.OwnerId, query.UserId);
            var organizationDocuments = await _repository.Collection.Find(filter).ToListAsync(cancellationToken);

            return organizationDocuments.Select(org => new OrganizationDto(org.AsEntity()));
        }
    }
}
