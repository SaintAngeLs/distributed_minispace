using Convey.Persistence.MongoDB;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Repositories;
using MiniSpace.Services.Comments.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Comments.Application.Wrappers;
using MiniSpace.Services.Comments.Core.Wrappers;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Repositories
{
    [ExcludeFromCodeCoverage]
    public class CommentMongoRepository : ICommentRepository
    {
        private readonly IMongoRepository<CommentDocument, Guid> _repository;

        public CommentMongoRepository(IMongoRepository<CommentDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Comment> GetAsync(Guid id)
        {
            var comment = await _repository.GetAsync(p => p.Id == id);
            return comment?.AsEntity();
        }

        public Task AddAsync(Comment comment)
            => _repository.AddAsync(comment.ToDocument());

        public Task UpdateAsync(Comment comment)
            => _repository.UpdateAsync(comment.ToDocument());

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);

        public async Task<IEnumerable<Comment>> GetByEventIdAsync(Guid eventId)
        {
            var comments = await _repository.Collection
                .AsQueryable()
                .Where(e => e.CommentContext == CommentContext.OrganizationEvent && e.ContextId == eventId)
                .ToListAsync();
            return comments.Select(e => e.AsEntity());
        }

        public async Task<IEnumerable<Comment>> GetByPostIdAsync(Guid postId)
        {
            var comments = await _repository.Collection
                .AsQueryable()
                .Where(e => e.CommentContext == CommentContext.OrganizationPost && e.ContextId == postId)
                .ToListAsync();
            return comments.Select(e => e.AsEntity());
        }

        public async Task<PagedResponse<Comment>> BrowseCommentsAsync(BrowseCommentsRequest request)
        {
            var filterDefinition = request.ParentId == Guid.Empty
                ? Extensions.ToFilterDefinition(request.ContextId, request.CommentContext).AddParentFilter()
                : Extensions.ToFilterDefinition(request.ContextId, request.CommentContext).AddChildrenFilter(request.ParentId);
            var sortDefinition = Extensions.ToSortDefinition(request.SortBy, request.SortDirection);

            var pagedEvents = await _repository.Collection.AggregateByPage(
                filterDefinition,
                sortDefinition,
                request.PageNumber,
                request.PageSize);

            var comments = pagedEvents.data.Select(e => e.AsEntity());
            return new PagedResponse<Comment>(comments, request.PageNumber, request.PageSize, pagedEvents.totalElements);
        }
    }
}
