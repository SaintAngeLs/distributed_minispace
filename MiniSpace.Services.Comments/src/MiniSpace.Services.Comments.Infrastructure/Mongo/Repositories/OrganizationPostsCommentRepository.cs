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
    public class OrganizationPostsCommentRepository : IOrganizationPostsCommentRepository
    {
        private readonly IMongoRepository<OrganizationPostCommentDocument, Guid> _repository;

        public OrganizationPostsCommentRepository(IMongoRepository<OrganizationPostCommentDocument, Guid> repository)
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
            var filter = Builders<OrganizationPostCommentDocument>.Filter.Eq(d => d.OrganizationPostId, comment.ContextId);
            var update = Builders<OrganizationPostCommentDocument>.Update.Push(d => d.Comments, comment.ToDocument());

            await _repository.Collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        public async Task UpdateAsync(Comment comment)
        {
            var filter = Builders<OrganizationPostCommentDocument>.Filter.And(
                Builders<OrganizationPostCommentDocument>.Filter.Eq(d => d.OrganizationPostId, comment.ContextId),
                Builders<OrganizationPostCommentDocument>.Filter.ElemMatch(d => d.Comments, c => c.Id == comment.Id)
            );

            var update = Builders<OrganizationPostCommentDocument>.Update
                .Set($"{nameof(OrganizationPostCommentDocument.Comments)}.$.{nameof(CommentDocument.TextContent)}", comment.TextContent)
                .Set($"{nameof(OrganizationPostCommentDocument.Comments)}.$.{nameof(CommentDocument.LastUpdatedAt)}", comment.LastUpdatedAt)
                .Set($"{nameof(OrganizationPostCommentDocument.Comments)}.$.{nameof(CommentDocument.IsDeleted)}", comment.IsDeleted);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<OrganizationPostCommentDocument>.Filter.ElemMatch(d => d.Comments, c => c.Id == id);
            var update = Builders<OrganizationPostCommentDocument>.Update.PullFilter(d => d.Comments, c => c.Id == id);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task<IEnumerable<Comment>> GetByPostIdAsync(Guid postId)
        {
            var doc = await _repository.GetAsync(d => d.OrganizationPostId == postId);
            return doc?.Comments.Select(c => c.AsEntity()) ?? Enumerable.Empty<Comment>();
        }

        public async Task<PagedResponse<Comment>> BrowseCommentsAsync(BrowseCommentsRequest request)
        {
            var filterDefinition = Builders<OrganizationPostCommentDocument>.Filter.Eq(d => d.OrganizationPostId, request.ContextId);
            var sortDefinition = OrganizationPostCommentExtensions.ToSortDefinition(request.SortBy, request.SortDirection);

            var pagedEvents = await _repository.Collection.AggregateByPage(
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
