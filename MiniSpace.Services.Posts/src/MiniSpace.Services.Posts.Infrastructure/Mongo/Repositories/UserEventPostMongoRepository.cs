using System.Diagnostics.CodeAnalysis;
using Convey.Persistence.MongoDB;
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
    public class UserEventPostMongoRepository : IUserEventPostRepository
    {
        private readonly IMongoRepository<UserEventPostDocument, Guid> _repository;

        public UserEventPostMongoRepository(IMongoRepository<UserEventPostDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Post> GetAsync(Guid id)
        {
            var userEventPost = await _repository.GetAsync(e => e.Id == id);
            return userEventPost?.UserEventPosts.FirstOrDefault(p => p.Id == id)?.AsEntity();
        }

        public async Task<IEnumerable<Post>> GetToUpdateAsync()
        {
            var postsToUpdate = await _repository.Collection.AsQueryable()
                .SelectMany(o => o.UserEventPosts)
                .Where(e => e.State == State.ToBePublished || e.State == State.Published)
                .ToListAsync();

            return postsToUpdate.Select(p => p.AsEntity());
        }

        public async Task<IEnumerable<Post>> GetByEventIdAsync(Guid eventId)
        {
            var posts = await _repository.Collection.AsQueryable()
                .Where(o => o.UserEventPosts.Any(p => p.EventId == eventId))
                .SelectMany(o => o.UserEventPosts.Where(p => p.EventId == eventId))
                .ToListAsync();

            return posts.Select(p => p.AsEntity());
        }

        public async Task<IEnumerable<Post>> GetByUserEventIdAsync(Guid userId, Guid eventId)
        {
            var posts = await _repository.Collection.AsQueryable()
                .Where(o => o.UserId == userId && o.UserEventPosts.Any(p => p.EventId == eventId))
                .SelectMany(o => o.UserEventPosts.Where(p => p.EventId == eventId))
                .ToListAsync();

            return posts.Select(p => p.AsEntity());
        } 

        public Task UpdateAsync(Post post)
        {
            var filter = Builders<UserEventPostDocument>.Filter.And(
                Builders<UserEventPostDocument>.Filter.Eq(o => o.UserId, post.UserId),
                Builders<UserEventPostDocument>.Filter.Eq("UserEventPosts.Id", post.Id)); 

            var update = Builders<UserEventPostDocument>.Update
                .Set("UserEventPosts.$", post.AsDocument()); 

            return _repository.Collection.UpdateOneAsync(filter, update);
        }



        public Task AddAsync(Post post)
        {
            var userEventPost = new UserEventPost(post.Id, post.UserId.Value, new List<Post> { post });
            return _repository.AddAsync(userEventPost.AsDocument());
        }


        public Task DeleteAsync(Guid id)
        {
            var filter = Builders<UserEventPostDocument>.Filter.ElemMatch(o => o.UserEventPosts, p => p.Id == id);
            var update = Builders<UserEventPostDocument>.Update.PullFilter(o => o.UserEventPosts, p => p.Id == id);

            return _repository.Collection.UpdateOneAsync(filter, update);
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return _repository.ExistsAsync(o => o.UserEventPosts.Any(p => p.Id == id));
        }

        public async Task<PagedResponse<Post>> BrowseUserEventPostsAsync(BrowseRequest request)
        {
            var filterDefinition = Builders<UserEventPostDocument>.Filter.Where(
                o => o.UserId == request.UserId && o.UserEventPosts.Any(p => p.EventId == request.EventId));

            var sortDefinition = UserEventPostExtensions.ToSortDefinition(request.SortBy, request.Direction); 

            var pagedEvents = await _repository.Collection.AggregateByPage<UserEventPostDocument>(filterDefinition, sortDefinition, request.PageNumber, request.PageSize);

            var posts = pagedEvents.data.SelectMany(o => o.UserEventPosts.Where(p => p.EventId == request.EventId)).ToList();

            return new PagedResponse<Post>(posts.Select(p => p.AsEntity()), request.PageNumber, request.PageSize, pagedEvents.totalElements);
        }
    }
}
