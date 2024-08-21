using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Extensions;
using MongoDB.Driver;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Repositories
{
    public class ReactionsOrganizationsPostMongoRepository : IReactionsOrganizationsPostRepository
    {
        private readonly IMongoRepository<OrganizationPostReactionDocument, Guid> _repository;

        public ReactionsOrganizationsPostMongoRepository(IMongoRepository<OrganizationPostReactionDocument, Guid> repository)
        {
            _repository = repository;
        }

        public Task<bool> ExistsAsync(Guid id)
            => _repository.ExistsAsync(x => x.OrganizationPostId == id);

        public async Task<Reaction> GetByIdAsync(Guid id)
        {
            var document = await _repository.GetAsync(x => x.Reactions.Any(r => r.Id == id));
            return document?.Reactions.FirstOrDefault(r => r.Id == id)?.ToEntity();
        }

        public async Task AddAsync(Reaction reaction)
        {
            var filter = Builders<OrganizationPostReactionDocument>.Filter.Eq(d => d.OrganizationPostId, reaction.ContentId);

            var update = Builders<OrganizationPostReactionDocument>.Update.Combine(
                Builders<OrganizationPostReactionDocument>.Update.Push(d => d.Reactions, reaction.AsDocument()),
                Builders<OrganizationPostReactionDocument>.Update.SetOnInsert(d => d.OrganizationPostId, reaction.ContentId)
            );

            var result = await _repository.Collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            if (!result.IsAcknowledged || result.ModifiedCount == 0)
            {
            }
        }


        public async Task UpdateAsync(Reaction reaction)
        {
            var filter = Builders<OrganizationPostReactionDocument>.Filter.And(
                Builders<OrganizationPostReactionDocument>.Filter.Eq(d => d.OrganizationPostId, reaction.ContentId),
                Builders<OrganizationPostReactionDocument>.Filter.ElemMatch(d => d.Reactions, r => r.Id == reaction.Id)
            );

            var update = Builders<OrganizationPostReactionDocument>.Update
                .Set(d => d.Reactions[-1], reaction.AsDocument());

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<OrganizationPostReactionDocument>.Filter.ElemMatch(d => d.Reactions, r => r.Id == id);
            var update = Builders<OrganizationPostReactionDocument>.Update
                .PullFilter(d => d.Reactions, r => r.Id == id);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task<IEnumerable<Reaction>> GetByContentIdAsync(Guid contentId)
        {
            var document = await _repository.GetAsync(d => d.OrganizationPostId == contentId);
            return document?.Reactions.Select(r => r.AsEntity()) ?? Enumerable.Empty<Reaction>();
        }
    }
}
