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
    public class ReactionsUserEventCommentsMongoRepository : IReactionsUserEventCommentsRepository
    {
        private readonly IMongoRepository<UserEventCommentsReactionDocument, Guid> _repository;

        public ReactionsUserEventCommentsMongoRepository(IMongoRepository<UserEventCommentsReactionDocument, Guid> repository)
        {
            _repository = repository;
        }

        public Task<bool> ExistsAsync(Guid id)
            => _repository.ExistsAsync(x => x.UserEventCommentId == id);

        public async Task<Reaction> GetByIdAsync(Guid id)
        {
            var document = await _repository.GetAsync(x => x.Reactions.Any(r => r.Id == id));
            return document?.Reactions.FirstOrDefault(r => r.Id == id)?.AsEntity();
        }

        public async Task AddAsync(Reaction reaction)
        {
            var filter = Builders<UserEventCommentsReactionDocument>.Filter.Eq(d => d.UserEventCommentId, reaction.ContentId);

            var update = Builders<UserEventCommentsReactionDocument>.Update.Combine(
                Builders<UserEventCommentsReactionDocument>.Update.Push(d => d.Reactions, reaction.AsDocument()),
                Builders<UserEventCommentsReactionDocument>.Update.SetOnInsert(d => d.UserEventCommentId, reaction.ContentId),
                Builders<UserEventCommentsReactionDocument>.Update.SetOnInsert(d => d.Id, Guid.NewGuid())
            );

            var options = new UpdateOptions { IsUpsert = true };
            var result = await _repository.Collection.UpdateOneAsync(filter, update, options);

            if (!result.IsAcknowledged || result.ModifiedCount == 0)
            {
            }
        }


        public async Task UpdateAsync(Reaction reaction)
        {
            var filter = Builders<UserEventCommentsReactionDocument>.Filter.And(
                Builders<UserEventCommentsReactionDocument>.Filter.Eq(x => x.UserEventCommentId, reaction.ContentId),
                Builders<UserEventCommentsReactionDocument>.Filter.ElemMatch(x => x.Reactions, r => r.Id == reaction.Id)
            );

            var update = Builders<UserEventCommentsReactionDocument>.Update.Set(x => x.Reactions[-1], reaction.AsDocument());

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<UserEventCommentsReactionDocument>.Filter.ElemMatch(x => x.Reactions, r => r.Id == id);
            var update = Builders<UserEventCommentsReactionDocument>.Update.PullFilter(x => x.Reactions, r => r.Id == id);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task<IEnumerable<Reaction>> GetByContentIdAsync(Guid contentId)
        {
            var document = await _repository.GetAsync(d => d.UserEventCommentId == contentId);
            return document?.Reactions.Select(r => r.AsEntity()) ?? Enumerable.Empty<Reaction>();
        }

        public async Task<Reaction> GetAsync(Guid contentId, Guid userId)
        {
            var filter = Builders<UserEventCommentsReactionDocument>.Filter.And(
                Builders<UserEventCommentsReactionDocument>.Filter.Eq(x => x.UserEventCommentId, contentId),
                Builders<UserEventCommentsReactionDocument>.Filter.ElemMatch(x => x.Reactions, r => r.UserId == userId)
            );

            var document = await _repository.Collection.Find(filter).FirstOrDefaultAsync();
            return document?.Reactions.FirstOrDefault(r => r.UserId == userId)?.AsEntity();
        }
    }
}
