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
    public class ReactionsUserPostCommentsMongoRepository : IReactionsUserPostCommentsRepository
    {
        private readonly IMongoRepository<UserPostCommentsReactionDocument, Guid> _repository;

        public ReactionsUserPostCommentsMongoRepository(IMongoRepository<UserPostCommentsReactionDocument, Guid> repository)
        {
            _repository = repository;
        }

        public Task<bool> ExistsAsync(Guid id)
            => _repository.ExistsAsync(x => x.UserPostCommentId == id);

        public async Task<Reaction> GetByIdAsync(Guid id)
        {
            var document = await _repository.GetAsync(x => x.Reactions.Any(r => r.Id == id));
            return document?.Reactions.FirstOrDefault(r => r.Id == id)?.AsEntity();
        }

       public async Task AddAsync(Reaction reaction)
        {
            // Ensure the document's Id is set to the reaction's Id
            var filter = Builders<UserPostCommentsReactionDocument>.Filter.Eq(x => x.UserPostCommentId, reaction.ContentId);

            var update = Builders<UserPostCommentsReactionDocument>.Update.Combine(
                Builders<UserPostCommentsReactionDocument>.Update.Push(x => x.Reactions, reaction.AsDocument()),
                Builders<UserPostCommentsReactionDocument>.Update.SetOnInsert(x => x.UserPostCommentId, reaction.ContentId),
                Builders<UserPostCommentsReactionDocument>.Update.SetOnInsert(x => x.Id, reaction.ContentId) // Set the Id to ensure it's not an ObjectId
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
            var filter = Builders<UserPostCommentsReactionDocument>.Filter.And(
                Builders<UserPostCommentsReactionDocument>.Filter.Eq(x => x.UserPostCommentId, reaction.ContentId),
                Builders<UserPostCommentsReactionDocument>.Filter.ElemMatch(x => x.Reactions, r => r.Id == reaction.Id)
            );

            var update = Builders<UserPostCommentsReactionDocument>.Update.Set(x => x.Reactions[-1], reaction.AsDocument());

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<UserPostCommentsReactionDocument>.Filter.ElemMatch(x => x.Reactions, r => r.Id == id);
            var update = Builders<UserPostCommentsReactionDocument>.Update.PullFilter(x => x.Reactions, r => r.Id == id);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task<IEnumerable<Reaction>> GetByContentIdAsync(Guid contentId)
        {
            var document = await _repository.GetAsync(d => d.UserPostCommentId == contentId);
            return document?.Reactions.Select(r => r.AsEntity()) ?? Enumerable.Empty<Reaction>();
        }

        public async Task<Reaction> GetAsync(Guid contentId, Guid userId)
        {
            var filter = Builders<UserPostCommentsReactionDocument>.Filter.And(
                Builders<UserPostCommentsReactionDocument>.Filter.Eq(x => x.UserPostCommentId, contentId),
                Builders<UserPostCommentsReactionDocument>.Filter.ElemMatch(x => x.Reactions, r => r.UserId == userId)
            );

            var document = await _repository.Collection.Find(filter).FirstOrDefaultAsync();
            return document?.Reactions.FirstOrDefault(r => r.UserId == userId)?.AsEntity();
        }
    }
}
