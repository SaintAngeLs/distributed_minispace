using Convey.Persistence.MongoDB;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Repositories
{
    public class OrganizationMongoRepository : IOrganizationRepository
    {
        private readonly IMongoRepository<OrganizationDocument, Guid> _repository;

        public OrganizationMongoRepository(IMongoRepository<OrganizationDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<Organization> GetAsync(Guid id)
        {
            var student = await _repository.GetAsync(o => o.Id == id);

            return student?.AsEntity();
        }

        public Task AddAsync(Organization student)
            => _repository.AddAsync(student.AsDocument());

        public Task UpdateAsync(Organization student)
            => _repository.UpdateAsync(student.AsDocument());

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
    }    
}