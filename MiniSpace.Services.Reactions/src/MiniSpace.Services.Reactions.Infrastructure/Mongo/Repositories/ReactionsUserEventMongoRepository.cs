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
    public class ReactionsUserEventMongoRepository : IReactionsUserEventRepository
    {
        private readonly IMongoRepository<UserEventReactionDocument, Guid> _repository;

        public ReactionsUserEventMongoRepository(IMongoRepository<UserEventReactionDocument, Guid> repository)
        {
            _repository = repository;
        }

        public Task<bool> ExistsAsync(Guid id)
            => _repository.ExistsAsync(x => x.UserEventId == id);

        public async Task<Reaction> GetByIdAsync(Guid id)
        {
            var document = await _repository.GetAsync(x => x.Reactions.Any(r => r.Id == id));
            return document?.Reactions.FirstOrDefault(r => r.Id == id)?.ToEntity();
        }

        public async Task AddAsync(Reaction reaction)
        {
            var filter = Builders<UserEventReactionDocument>.Filter.Eq(d => d.UserEventId, reaction.ContentId);

            var update = Builders<UserEventReactionDocument>.Update.Combine(
                Builders<UserEventReactionDocument>.Update.Push(d => d.Reactions, reaction.AsDocument()),
                Builders<UserEventReactionDocument>.Update.SetOnInsert(d => d.UserEventId, reaction.ContentId),
                Builders<UserEventReactionDocument>.Update.SetOnInsert(d => d.Id, Guid.NewGuid()) // Ensure the document Id is a new Guid
            );

            var options = new UpdateOptions { IsUpsert = true };
            var result = await _repository.Collection.UpdateOneAsync(filter, update, options);

            if (!result.IsAcknowledged || result.ModifiedCount == 0)
            {
            }
        }


        public async Task UpdateAsync(Reaction reaction)
        {
            var filter = Builders<UserEventReactionDocument>.Filter.And(
                Builders<UserEventReactionDocument>.Filter.Eq(d => d.UserEventId, reaction.ContentId),
                Builders<UserEventReactionDocument>.Filter.ElemMatch(d => d.Reactions, r => r.Id == reaction.Id)
            );

            var update = Builders<UserEventReactionDocument>.Update
                .Set(d => d.Reactions[-1], reaction.AsDocument());

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<UserEventReactionDocument>.Filter.ElemMatch(d => d.Reactions, r => r.Id == id);
            var update = Builders<UserEventReactionDocument>.Update
                .PullFilter(d => d.Reactions, r => r.Id == id);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task<IEnumerable<Reaction>> GetByContentIdAsync(Guid contentId)
        {
            var document = await _repository.GetAsync(d => d.UserEventId == contentId);
            return document?.Reactions.Select(r => r.AsEntity()) ?? Enumerable.Empty<Reaction>();
        }

        public async Task<Reaction> GetAsync(Guid contentId, Guid userId)
        {
            var filter = Builders<UserEventReactionDocument>.Filter.And(
                Builders<UserEventReactionDocument>.Filter.Eq(x => x.UserEventId, contentId),
                Builders<UserEventReactionDocument>.Filter.ElemMatch(x => x.Reactions, r => r.UserId == userId)
            );

            var document = await _repository.Collection.Find(filter).FirstOrDefaultAsync();
            return document?.Reactions.FirstOrDefault(r => r.UserId == userId)?.AsEntity();
        }
    }
}
