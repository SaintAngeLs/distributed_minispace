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
    public class ReactionsUserPostMongoRepository : IReactionsUserPostRepository
    {
        private readonly IMongoRepository<UserPostReactionDocument, Guid> _repository;

        public ReactionsUserPostMongoRepository(IMongoRepository<UserPostReactionDocument, Guid> repository)
        {
            _repository = repository;
        }

        public Task<bool> ExistsAsync(Guid id)
            => _repository.ExistsAsync(x => x.UserPostId == id);

        public async Task<Reaction> GetByIdAsync(Guid id)
        {
            var document = await _repository.GetAsync(x => x.Reactions.Any(r => r.Id == id));
            return document?.Reactions.FirstOrDefault(r => r.Id == id)?.AsEntity();
        }

        public async Task AddAsync(Reaction reaction)
        {
            var filter = Builders<UserPostReactionDocument>.Filter.Eq(x => x.UserPostId, reaction.ContentId);

            var update = Builders<UserPostReactionDocument>.Update.Combine(
                Builders<UserPostReactionDocument>.Update.Push(x => x.Reactions, reaction.AsDocument()),
                Builders<UserPostReactionDocument>.Update.SetOnInsert(x => x.UserPostId, reaction.ContentId),
                Builders<UserPostReactionDocument>.Update.SetOnInsert(x => x.Id, Guid.NewGuid()) // Ensure the document Id is a new Guid
            );

            var options = new UpdateOptions { IsUpsert = true }; // Upsert: insert if not exists, update if exists
            var result = await _repository.Collection.UpdateOneAsync(filter, update, options);

            if (!result.IsAcknowledged || result.ModifiedCount == 0)
            {
                // Handle the case where the update wasn't acknowledged or nothing was modified
                // This could involve logging or throwing an exception based on your application's needs
            }
        }

        public async Task UpdateAsync(Reaction reaction)
        {
            var filter = Builders<UserPostReactionDocument>.Filter.And(
                Builders<UserPostReactionDocument>.Filter.Eq(x => x.UserPostId, reaction.ContentId),
                Builders<UserPostReactionDocument>.Filter.ElemMatch(x => x.Reactions, r => r.Id == reaction.Id)
            );

            var update = Builders<UserPostReactionDocument>.Update.Set(x => x.Reactions[-1], reaction.AsDocument());

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<UserPostReactionDocument>.Filter.ElemMatch(x => x.Reactions, r => r.Id == id);
            var update = Builders<UserPostReactionDocument>.Update.PullFilter(x => x.Reactions, r => r.Id == id);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task<IEnumerable<Reaction>> GetByContentIdAsync(Guid contentId)
        {
            var filter = Builders<UserPostReactionDocument>.Filter.Eq(d => d.UserPostId, contentId);
            var document = await _repository.Collection.Find(filter).FirstOrDefaultAsync();

            return document?.Reactions.Select(r => r.AsEntity()) ?? Enumerable.Empty<Reaction>();
        }

        public async Task<Reaction> GetAsync(Guid contentId, Guid userId)
        {
            var filter = Builders<UserPostReactionDocument>.Filter.And(
                Builders<UserPostReactionDocument>.Filter.Eq(x => x.UserPostId, contentId),
                Builders<UserPostReactionDocument>.Filter.ElemMatch(x => x.Reactions, r => r.UserId == userId)
            );

            var document = await _repository.Collection.Find(filter).FirstOrDefaultAsync();
            return document?.Reactions.FirstOrDefault(r => r.UserId == userId)?.AsEntity();
        }

    }
}
