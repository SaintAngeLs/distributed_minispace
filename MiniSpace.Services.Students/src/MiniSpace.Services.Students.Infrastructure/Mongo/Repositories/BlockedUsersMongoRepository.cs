using Paralax.Persistence.MongoDB;
using MongoDB.Driver;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Repositories
{
    public class BlockedUsersMongoRepository : IBlockedUsersRepository
    {
        private readonly IMongoRepository<BlockedUsersDocument, Guid> _repository;

        public BlockedUsersMongoRepository(IMongoRepository<BlockedUsersDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<BlockedUsers> GetAsync(Guid blockerId)
        {
            var document = await _repository.GetAsync(d => d.UserId == blockerId);
            return document?.AsEntity();
        }

        public async Task AddAsync(BlockedUsers blockedUsers)
        {
            var document = blockedUsers.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task UpdateAsync(BlockedUsers blockedUsers)
        {
            var document = blockedUsers.AsDocument();
            var filter = Builders<BlockedUsersDocument>.Filter.Eq(d => d.UserId, document.UserId);

            // Replace the existing document
            var result = await _repository.Collection.ReplaceOneAsync(filter, document, new ReplaceOptions { IsUpsert = true });

            // Ensure the operation was successful
            if (result.MatchedCount == 0 && result.UpsertedId == null)
            {
                throw new Exception("Failed to update the blocked user list.");
            }
        }

        public async Task DeleteAsync(Guid blockerId)
        {
            var filter = Builders<BlockedUsersDocument>.Filter.Eq(d => d.UserId, blockerId);
            await _repository.Collection.DeleteOneAsync(filter);
        }
    }
}
