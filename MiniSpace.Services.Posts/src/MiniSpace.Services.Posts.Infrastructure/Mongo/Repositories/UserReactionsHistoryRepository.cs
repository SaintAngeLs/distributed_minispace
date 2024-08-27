using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Repositories;
using MiniSpace.Services.Posts.Core.Wrappers;
using MiniSpace.Services.Posts.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Repositories
{
    public class UserReactionsHistoryRepository : IUserReactionsHistoryRepository
    {
        private readonly IMongoCollection<UserReactionDocument> _collection;

        public UserReactionsHistoryRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<UserReactionDocument>("user_reactions_history");
        }

        public async Task SaveReactionAsync(Guid userId, Reaction reaction)
        {
            var filter = Builders<UserReactionDocument>.Filter.Eq(ur => ur.UserId, userId);

            var reactionDocument = reaction.AsDocument();

            var update = Builders<UserReactionDocument>.Update.Combine(
                Builders<UserReactionDocument>.Update.Push(ur => ur.Reactions, reactionDocument),
                Builders<UserReactionDocument>.Update.SetOnInsert(ur => ur.UserId, userId),
                Builders<UserReactionDocument>.Update.SetOnInsert(ur => ur.Id, Guid.NewGuid())
            );

            await _collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        public async Task<IEnumerable<Reaction>> GetUserReactionsAsync(Guid userId)
        {
            var userReactionsDocument = await _collection.Find(ur => ur.UserId == userId).FirstOrDefaultAsync();
            return userReactionsDocument?.AsEntities() ?? Enumerable.Empty<Reaction>();
        }

        public async Task<PagedResponse<Reaction>> GetUserReactionsPagedAsync(Guid userId, int pageNumber, int pageSize)
        {
            var userReactionsDocument = await _collection.Find(ur => ur.UserId == userId).FirstOrDefaultAsync();

            if (userReactionsDocument == null)
            {
                return new PagedResponse<Reaction>(Enumerable.Empty<Reaction>(), pageNumber, pageSize, 0);
            }

            var pagedReactions = userReactionsDocument.Reactions
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(doc => doc.AsEntity());

            return new PagedResponse<Reaction>(pagedReactions, pageNumber, pageSize, userReactionsDocument.Reactions.Count());
        }

        public async Task DeleteReactionAsync(Guid userId, Guid reactionId)
        {
            var filter = Builders<UserReactionDocument>.Filter.And(
                Builders<UserReactionDocument>.Filter.Eq(ur => ur.UserId, userId),
                Builders<UserReactionDocument>.Filter.ElemMatch(ur => ur.Reactions, r => r.Id == reactionId)
            );

            var update = Builders<UserReactionDocument>.Update.PullFilter(ur => ur.Reactions, r => r.Id == reactionId);
            await _collection.UpdateOneAsync(filter, update);
        }
    }
}
