using Paralax.CQRS.Queries;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;
using System.Diagnostics.CodeAnalysis;


namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
     public class GetOrganizationHandler : IQueryHandler<GetOrganization, OrganizationDto>
    {
        private readonly IMongoRepository<OrganizationDocument, Guid> _repository;

        public GetOrganizationHandler(IMongoRepository<OrganizationDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<OrganizationDto> HandleAsync(GetOrganization query, CancellationToken cancellationToken)
        {
            var organizationDocument = await _repository.GetAsync(o => o.Id == query.OrganizationId);
            if (organizationDocument == null)
            {
                return null;
            }

            var organization = organizationDocument.AsEntity();
            return new OrganizationDto(organization);
        }
    }
}