using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Repositories
{
    public class OrganizerMongoRepository : IOrganizerRepository
    {
        private readonly IMongoRepository<OrganizerDocument, Guid> _repository;

        public OrganizerMongoRepository(IMongoRepository<OrganizerDocument, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<Organizer> GetAsync(Guid id)
        {
            var organizer = await _repository.GetAsync(o => o.Id == id);

            return organizer?.AsEntity();
        }
        public Task<bool> ExistsAsync(Guid id) => _repository.ExistsAsync(o => o.Id == id);
        public Task AddAsync(Organizer organizer) => _repository.AddAsync(organizer.AsDocument());
        public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);
    }
}