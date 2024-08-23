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
    public class UserCommentsHistoryRepository : IUserCommentsHistoryRepository
    {
        private readonly IMongoCollection<UserCommentsDocument> _collection;

        public UserCommentsHistoryRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<UserCommentsDocument>("user_comments");
        }

        public async Task SaveCommentAsync(Guid userId, Comment comment)
        {
            var filter = Builders<UserCommentsDocument>.Filter.Eq(uc => uc.UserId, userId);
            var update = Builders<UserCommentsDocument>.Update.Push(uc => uc.Comments, comment.AsDocument());

            await _collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        public async Task<IEnumerable<Comment>> GetUserCommentsAsync(Guid userId)
        {
            var userCommentsDocument = await _collection.Find(uc => uc.UserId == userId).FirstOrDefaultAsync();
            return userCommentsDocument?.AsEntities() ?? Enumerable.Empty<Comment>();
        }

        public async Task<PagedResponse<Comment>> GetUserCommentsPagedAsync(Guid userId, int pageNumber, int pageSize)
        {
            var userCommentsDocument = await _collection.Find(uc => uc.UserId == userId).FirstOrDefaultAsync();

            if (userCommentsDocument == null)
            {
                return new PagedResponse<Comment>(Enumerable.Empty<Comment>(), pageNumber, pageSize, 0);
            }

            var pagedComments = userCommentsDocument.Comments
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(doc => doc.AsEntity());

            return new PagedResponse<Comment>(pagedComments, pageNumber, pageSize, userCommentsDocument.Comments.Count());
        }

        public async Task DeleteCommentAsync(Guid userId, Guid commentId)
        {
            var filter = Builders<UserCommentsDocument>.Filter.And(
                Builders<UserCommentsDocument>.Filter.Eq(uc => uc.UserId, userId),
                Builders<UserCommentsDocument>.Filter.ElemMatch(uc => uc.Comments, c => c.Id == commentId)
            );

            var update = Builders<UserCommentsDocument>.Update.PullFilter(uc => uc.Comments, c => c.Id == commentId);
            await _collection.UpdateOneAsync(filter, update);
        }
    }
}
