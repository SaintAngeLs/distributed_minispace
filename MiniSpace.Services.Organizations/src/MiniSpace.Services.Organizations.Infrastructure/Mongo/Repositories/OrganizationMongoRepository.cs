using Convey.Persistence.MongoDB;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Repositories
{
    [ExcludeFromCodeCoverage]
    public class OrganizationMongoRepository : IOrganizationRepository
    {
        private readonly IMongoRepository<OrganizationDocument, Guid> _repository;

        public OrganizationMongoRepository(IMongoRepository<OrganizationDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<Organization> GetAsync(Guid id)
        {
            var organization = await _repository.GetAsync(o => o.Id == id);

            return organization?.AsEntity();
        }
        
        public async Task<IEnumerable<Organization>> GetOrganizerOrganizationsAsync(Guid organizerId)
        {
            var organizations = await _repository.FindAsync(o 
                => o.Organizers.Any(x => x.Id == organizerId));

            return organizations?.Select(o => o.AsEntity());
        }

        public Task AddAsync(Organization organization)
            => _repository.AddAsync(organization.AsDocument());

        public Task UpdateAsync(Organization organization)
            => _repository.UpdateAsync(organization.AsDocument());

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
    }    
}