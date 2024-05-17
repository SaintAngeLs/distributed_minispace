using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Organizations.Application;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Queries.Handlers
{
    public class GetOrganizerOrganizationsHandler : IQueryHandler<GetOrganizerOrganizations, IEnumerable<OrganizationDto>>
    {
        private readonly IMongoRepository<OrganizationDocument, Guid> _repository;
        private readonly IAppContext _appContext;

        public GetOrganizerOrganizationsHandler(IMongoRepository<OrganizationDocument, Guid> repository, IAppContext appContext)
        {
            _repository = repository;
            _appContext = appContext;
        }

        public async Task<IEnumerable<OrganizationDto>> HandleAsync(GetOrganizerOrganizations query, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if ((identity.Id != query.OrganizerId || !identity.IsOrganizer) && !identity.IsAdmin)
            {
                return Enumerable.Empty<OrganizationDto>();
            }
            
            var roots = (await _repository.FindAsync(o => true)).Select(o =>o.AsEntity());
            var organizerOrganizations = new List<Organization>();
            foreach (var root in roots)
            {
                var organizations = Organization.FindOrganizations(query.OrganizerId, root);
                organizerOrganizations.AddRange(organizations);
            }

            return organizerOrganizations.Select(o => new OrganizationDto(o));
        }
    }
}