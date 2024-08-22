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

            var update = Builders<OrganizationPostCommentDocument>.Update.Combine(
                Builders<OrganizationPostCommentDocument>.Update.Push(d => d.Comments, comment.ToDocument()),
                Builders<OrganizationPostCommentDocument>.Update.SetOnInsert(d => d.OrganizationPostId, comment.ContextId),
                Builders<OrganizationPostCommentDocument>.Update.SetOnInsert(d => d.Id, Guid.NewGuid())
            );

            var options = new UpdateOptions { IsUpsert = true };
            var result = await _repository.Collection.UpdateOneAsync(filter, update, options);

            if (!result.IsAcknowledged || result.ModifiedCount == 0)
            {
                Console.Error.WriteLine("Failed to add or update the comment.");
            }
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
            // Log the ContextId to ensure it is correct
            Console.WriteLine($"Searching for OrganizationPostId: {request.ContextId}");

            // Filter to find the document matching the provided OrganizationPostId (ContextId)
            var filterDefinition = Builders<OrganizationPostCommentDocument>.Filter.Eq(d => d.OrganizationPostId, request.ContextId);

            // Fetch the document(s) that match the filter
            var document = await _repository.Collection.Find(filterDefinition).FirstOrDefaultAsync();

            // Log the document found or not found
            if (document == null)
            {
                Console.WriteLine("No document found with the specified OrganizationPostId.");
                return new PagedResponse<Comment>(Enumerable.Empty<Comment>(), request.PageNumber, request.PageSize, 0);
            }

            // Log the found document details
            Console.WriteLine($"Found document: ID = {document.Id}, OrganizationPostId = {document.OrganizationPostId}");

            // Extract the comments from the document and map them to the Comment entity
            var comments = document.Comments.Select(c => c.AsEntity()).ToList();

            // Log the IDs of all comments retrieved
            if (comments.Any())
            {
                Console.WriteLine("Comments found:");
                foreach (var comment in comments)
                {
                    Console.WriteLine($"Comment ID: {comment.Id}");
                }
            }
            else
            {
                Console.WriteLine("No comments found.");
            }

            // Return the comments as a PagedResponse
            return new PagedResponse<Comment>(comments, request.PageNumber, request.PageSize, comments.Count);
        }
    }
}
