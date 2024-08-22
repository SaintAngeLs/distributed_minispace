using Convey.Persistence.MongoDB;
using MiniSpace.Services.Comments.Application.Wrappers;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Repositories;
using MiniSpace.Services.Comments.Core.Wrappers;
using MiniSpace.Services.Comments.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Repositories
{
    public class UserPostsCommentRepository : IUserPostsCommentRepository
    {
        private readonly IMongoRepository<UserPostCommentDocument, Guid> _repository;

        public UserPostsCommentRepository(IMongoRepository<UserPostCommentDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Comment> GetAsync(Guid id)
        {
            var doc = await _repository.GetAsync(d => d.Comments.Any(c => c.Id == id));
            return doc?.Comments.FirstOrDefault(c => c.Id == id)?.AsEntity();
        }

        public async Task AddAsync(Comment comment)
        {
            var filter = Builders<UserPostCommentDocument>.Filter.Eq(d => d.UserPostId, comment.ContextId);

            var update = Builders<UserPostCommentDocument>.Update.Combine(
                Builders<UserPostCommentDocument>.Update.Push(d => d.Comments, comment.ToDocument()), 
                Builders<UserPostCommentDocument>.Update.SetOnInsert(d => d.UserPostId, comment.ContextId), 
                Builders<UserPostCommentDocument>.Update.SetOnInsert(d => d.Id, Guid.NewGuid())
            );

            var options = new UpdateOptions { IsUpsert = true };
            var result = await _repository.Collection.UpdateOneAsync(filter, update, options);

            if (!result.IsAcknowledged || result.ModifiedCount == 0)
            {
                // Handle the case whre the update or insert did not succeed
                Console.Error.WriteLine("Failed to add or update the comment.");
            }
        }


        public async Task UpdateAsync(Comment comment)
        {
            var filter = Builders<UserPostCommentDocument>.Filter.And(
                Builders<UserPostCommentDocument>.Filter.Eq(d => d.UserPostId, comment.ContextId),
                Builders<UserPostCommentDocument>.Filter.ElemMatch(d => d.Comments, c => c.Id == comment.Id)
            );

            var update = Builders<UserPostCommentDocument>.Update
                .Set($"{nameof(UserPostCommentDocument.Comments)}.$.{nameof(CommentDocument.TextContent)}", comment.TextContent)
                .Set($"{nameof(UserPostCommentDocument.Comments)}.$.{nameof(CommentDocument.LastUpdatedAt)}", comment.LastUpdatedAt)
                .Set($"{nameof(UserPostCommentDocument.Comments)}.$.{nameof(CommentDocument.IsDeleted)}", comment.IsDeleted);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<UserPostCommentDocument>.Filter.ElemMatch(d => d.Comments, c => c.Id == id);
            var update = Builders<UserPostCommentDocument>.Update.PullFilter(d => d.Comments, c => c.Id == id);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task<IEnumerable<Comment>> GetByPostIdAsync(Guid postId)
        {
            var doc = await _repository.GetAsync(d => d.UserPostId == postId);
            return doc?.Comments.Select(c => c.AsEntity()) ?? Enumerable.Empty<Comment>();
        }

        public async Task<PagedResponse<Comment>> BrowseCommentsAsync(BrowseCommentsRequest request)
        {
            var filterDefinition = Builders<UserPostCommentDocument>.Filter.Eq(d => d.UserPostId, request.ContextId);
            var sortDefinition = UserPostCommentExtensions.ToSortDefinition(request.SortBy, request.SortDirection);

            var pagedEvents = await _repository.Collection.AggregateByPage<UserPostCommentDocument>(
                filterDefinition,
                sortDefinition,
                request.PageNumber,
                request.PageSize
            );

            var comments = pagedEvents.data.SelectMany(d => d.Comments.Select(c => c.AsEntity()));
            return new PagedResponse<Comment>(comments, request.PageNumber, request.PageSize, pagedEvents.totalElements);
        }
    }
}
