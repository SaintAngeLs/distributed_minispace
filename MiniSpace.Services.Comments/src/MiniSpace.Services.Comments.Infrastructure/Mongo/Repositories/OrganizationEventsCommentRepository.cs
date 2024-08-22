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
    public class OrganizationEventsCommentRepository : IOrganizationEventsCommentRepository
    {
        private readonly IMongoRepository<OrganizationEventCommentDocument, Guid> _repository;

        public OrganizationEventsCommentRepository(IMongoRepository<OrganizationEventCommentDocument, Guid> repository)
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
            var filter = Builders<OrganizationEventCommentDocument>.Filter.Eq(d => d.OrganizationEventId, comment.ContextId);

            var update = Builders<OrganizationEventCommentDocument>.Update.Combine(
                Builders<OrganizationEventCommentDocument>.Update.Push(d => d.Comments, comment.ToDocument()), 
                Builders<OrganizationEventCommentDocument>.Update.SetOnInsert(d => d.OrganizationEventId, comment.ContextId), 
                Builders<OrganizationEventCommentDocument>.Update.SetOnInsert(d => d.Id, Guid.NewGuid())
            );

            var options = new UpdateOptions { IsUpsert = true };
            var result = await _repository.Collection.UpdateOneAsync(filter, update, options);

            if (!result.IsAcknowledged || result.ModifiedCount == 0)
            {
                // Handle the case where the update or insert did not succeed
                Console.Error.WriteLine("Failed to add or update the comment.");
            }
        }


        public async Task UpdateAsync(Comment comment)
        {
            var filter = Builders<OrganizationEventCommentDocument>.Filter.And(
                Builders<OrganizationEventCommentDocument>.Filter.Eq(d => d.OrganizationEventId, comment.ContextId),
                Builders<OrganizationEventCommentDocument>.Filter.ElemMatch(d => d.Comments, c => c.Id == comment.Id)
            );

            var update = Builders<OrganizationEventCommentDocument>.Update
                .Set($"{nameof(OrganizationEventCommentDocument.Comments)}.$.{nameof(CommentDocument.TextContent)}", comment.TextContent)
                .Set($"{nameof(OrganizationEventCommentDocument.Comments)}.$.{nameof(CommentDocument.LastUpdatedAt)}", comment.LastUpdatedAt)
                .Set($"{nameof(OrganizationEventCommentDocument.Comments)}.$.{nameof(CommentDocument.IsDeleted)}", comment.IsDeleted);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<OrganizationEventCommentDocument>.Filter.ElemMatch(d => d.Comments, c => c.Id == id);
            var update = Builders<OrganizationEventCommentDocument>.Update.PullFilter(d => d.Comments, c => c.Id == id);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public async Task<IEnumerable<Comment>> GetByEventIdAsync(Guid eventId)
        {
            var doc = await _repository.GetAsync(d => d.OrganizationEventId == eventId);
            return doc?.Comments.Select(c => c.AsEntity()) ?? Enumerable.Empty<Comment>();
        }

        public async Task<PagedResponse<Comment>> BrowseCommentsAsync(BrowseCommentsRequest request)
        {
            var filterDefinition = Builders<OrganizationEventCommentDocument>.Filter.Eq(d => d.OrganizationEventId, request.ContextId);
            var sortDefinition = OrganizationEventCommentExtensions.ToSortDefinition(request.SortBy, request.SortDirection);

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
