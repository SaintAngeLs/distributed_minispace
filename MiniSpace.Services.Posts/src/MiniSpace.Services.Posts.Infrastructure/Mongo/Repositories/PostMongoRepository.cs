using System.Collections;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Core.Repositories;
using MiniSpace.Services.Posts.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Repositories
{
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
        
        public async Task<(IEnumerable<Post> posts, int pageNumber,int pageSize, int totalPages, int totalElements)> BrowseCommentsAsync(int pageNumber, int pageSize, 
            IEnumerable<Guid> eventsIds, IEnumerable<string> sortBy, string direction)
        {
            var filterDefinition = Extensions.ToFilterDefinition(eventsIds);
            var sortDefinition = Extensions.ToSortDefinition(sortBy, direction);
            
            var pagedEvents = await BrowseAsync(filterDefinition, sortDefinition, pageNumber, pageSize);
            
            return (pagedEvents.data.Select(e => e.AsEntity()), pageNumber, pageSize,
                pagedEvents.totalPages, pagedEvents.totalElements);
        }
    }    
}
