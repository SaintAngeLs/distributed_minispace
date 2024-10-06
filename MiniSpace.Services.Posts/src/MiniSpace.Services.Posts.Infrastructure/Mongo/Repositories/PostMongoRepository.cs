using System.Diagnostics.CodeAnalysis;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Repositories;
using MiniSpace.Services.Posts.Core.Requests;
using MiniSpace.Services.Posts.Core.Wrappers;
using MiniSpace.Services.Posts.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Repositories
{
    [ExcludeFromCodeCoverage]
    public class PostMongoRepository : IPostRepository
    {
        private readonly IMongoRepository<PostDocument, Guid> _repository;

        public PostMongoRepository(IMongoRepository<PostDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<Post> GetAsync(Guid id)
        {
            var post = await _repository.GetAsync(p => p.Id == id);

            return post?.AsEntity();
        }

        public async Task<IEnumerable<Post>> GetToUpdateAsync()
        {
            var posts = _repository.Collection.AsQueryable();
            var postsToUpdate = await posts.Where(e 
                => e.State == State.ToBePublished || e.State == State.Published).ToListAsync();
            return postsToUpdate.Select(e => e.AsEntity());
        }
        
        public async Task<IEnumerable<Post>> GetByEventIdAsync(Guid eventId)
        {
            var posts = _repository.Collection.AsQueryable();
            var postsByEventId = await posts.Where(e => e.EventId == eventId).ToListAsync();
            return postsByEventId.Select(e => e.AsEntity());
        }
        
        public Task AddAsync(Post post)
            => _repository.AddAsync(post.AsDocument());

        public Task UpdateAsync(Post post)
            => _repository.UpdateAsync(post.AsDocument());

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
        
        public Task<bool> ExistsAsync(Guid id)
            => _repository.ExistsAsync(p => p.Id == id);

        private async Task<(int totalPages, int totalElements, IEnumerable<PostDocument> data)> BrowseAsync(
            FilterDefinition<PostDocument> filterDefinition, SortDefinition<PostDocument> sortDefinition, 
            int pageNumber, int pageSize)
        {
            var pagedEvents = await _repository.Collection.AggregateByPage(
                filterDefinition,
                sortDefinition,
                pageNumber,
                pageSize);

            return pagedEvents;
        }

        public async Task<PagedResponse<Post>> BrowsePostsAsync(BrowseRequest request)
        {
            var filterDefinition = Builders<PostDocument>.Filter.Empty;
            var sortDefinition = Extensions.ToSortDefinition(request.SortBy, request.Direction);

            var totalItems = await _repository.Collection.CountDocumentsAsync(filterDefinition);

            var posts = await _repository.Collection
                .Find(filterDefinition)
                .Sort(sortDefinition)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Limit(request.PageSize)
                .ToListAsync();

            var postEntities = posts.Select(p => p.AsEntity());

            return new PagedResponse<Post>(postEntities, request.PageNumber, request.PageSize, (int)totalItems);
        }
    }    
}
