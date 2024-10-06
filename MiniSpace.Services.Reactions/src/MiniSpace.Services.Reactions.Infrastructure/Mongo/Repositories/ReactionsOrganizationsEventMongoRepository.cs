using System;
using System.Threading.Tasks;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Extensions;
using MongoDB.Driver;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Repositories
{
    public class ReactionsOrganizationsEventMongoRepository : IReactionsOrganizationsEventRepository
    {
        private readonly IMongoRepository<OrganizationEventReactionDocument, Guid> _repository;

        public ReactionsOrganizationsEventMongoRepository(IMongoRepository<OrganizationEventReactionDocument, Guid> repository)
        {
            _repository = repository;
        }

        public Task<bool> ExistsAsync(Guid id)
            => _repository.ExistsAsync(x => x.OrganizationEventId == id);

        public async Task<Reaction> GetByIdAsync(Guid id)
        {
            var document = await _repository.GetAsync(x => x.Reactions.Any(r => r.Id == id));
            return document?.Reactions.FirstOrDefault(r => r.Id == id)?.ToEntity();
        }

        public async Task AddAsync(Reaction reaction)
        {
            var filter = Builders<OrganizationEventReactionDocument>.Filter.Eq(d => d.OrganizationEventId, reaction.ContentId);

            var update = Builders<OrganizationEventReactionDocument>.Update.Combine(
                Builders<OrganizationEventReactionDocument>.Update.Push(d => d.Reactions, reaction.AsDocument()),
                Builders<OrganizationEventReactionDocument>.Update.SetOnInsert(d => d.OrganizationEventId, reaction.ContentId),
                Builders<OrganizationEventReactionDocument>.Update.SetOnInsert(d => d.Id, Guid.NewGuid())
            );

            var options = new UpdateOptions { IsUpsert = true };
            var result = await _repository.Collection.UpdateOneAsync(filter, update, options);

            if (!result.IsAcknowledged || result.ModifiedCount == 0)
            {
            }
        }


        public async Task UpdateAsync(Reaction reaction)
        {
            var filter = Builders<OrganizationEventReactionDocument>.Filter.And(
                Builders<OrganizationEventReactionDocument>.Filter.Eq(d => d.OrganizationEventId, reaction.ContentId),
                Builders<OrganizationEventReactionDocument>.Filter.ElemMatch(d => d.Reactions, r => r.Id == reaction.Id)
            );

            var update = Builders<OrganizationEventReactionDocument>.Update
                .Set(d => d.Reactions[-1], reaction.AsDocument());

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<OrganizationEventReactionDocument>.Filter.ElemMatch(d => d.Reactions, r => r.Id == id);
            var update = Builders<OrganizationEventReactionDocument>.Update
                .PullFilter(d => d.Reactions, r => r.Id == id);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task<IEnumerable<Reaction>> GetByContentIdAsync(Guid contentId)
        {
            var document = await _repository.GetAsync(d => d.OrganizationEventId == contentId);
            return document?.Reactions.Select(r => r.AsEntity()) ?? Enumerable.Empty<Reaction>();
        }

        public async Task<Reaction> GetAsync(Guid contentId, Guid userId)
        {
            var filter = Builders<OrganizationEventReactionDocument>.Filter.And(
                Builders<OrganizationEventReactionDocument>.Filter.Eq(x => x.OrganizationEventId, contentId),
                Builders<OrganizationEventReactionDocument>.Filter.ElemMatch(x => x.Reactions, r => r.UserId == userId)
            );

            var document = await _repository.Collection.Find(filter).FirstOrDefaultAsync();
            return document?.Reactions.FirstOrDefault(r => r.UserId == userId)?.AsEntity();
        }
    }
}
