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
    public class UserPostMongoRepository : IUserPostRepository
    {
        private readonly IMongoRepository<UserPostDocument, Guid> _repository;

        public UserPostMongoRepository(IMongoRepository<UserPostDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Post> GetAsync(Guid id)
        {
            var userPost = await _repository.GetAsync(e => e.Id == id);
            return userPost?.UserPosts.FirstOrDefault(p => p.Id == id)?.AsEntity();
        }

        public async Task<IEnumerable<Post>> GetToUpdateAsync()
        {
            var postsToUpdate = await _repository.Collection.AsQueryable()
                .SelectMany(o => o.UserPosts)
                .Where(e => e.State == State.ToBePublished || e.State == State.Published)
                .ToListAsync();

            return postsToUpdate.Select(p => p.AsEntity());
        }

        public async Task<IEnumerable<Post>> GetByEventIdAsync(Guid eventId)
        {
            var posts = await _repository.Collection.AsQueryable()
                .Where(o => o.UserPosts.Any(p => p.EventId == eventId))
                .SelectMany(o => o.UserPosts.Where(p => p.EventId == eventId))
                .ToListAsync();

            return posts.Select(p => p.AsEntity());
        }

        public async Task<IEnumerable<Post>> GetByUserIdAsync(Guid userId)
        {
            var posts = await _repository.Collection.AsQueryable()
                .Where(o => o.UserId == userId)
                .SelectMany(o => o.UserPosts)
                .ToListAsync();

            return posts.Select(p => p.AsEntity());
        }

        public Task AddAsync(Post post)
        {
            var userPost = new UserPost(post.Id, post.UserId.Value, new List<Post> { post });
            return _repository.AddAsync(userPost.AsDocument());
        }

        public Task UpdateAsync(Post post)
        {
            var filter = Builders<UserPostDocument>.Filter.And(
                Builders<UserPostDocument>.Filter.Eq(o => o.UserId, post.UserId),
                Builders<UserPostDocument>.Filter.ElemMatch(o => o.UserPosts, p => p.Id == post.Id));

            var update = Builders<UserPostDocument>.Update.Set("UserPosts.$", post.AsDocument());

            return _repository.Collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(Guid id)
        {
            var filter = Builders<UserPostDocument>.Filter.ElemMatch(o => o.UserPosts, p => p.Id == id);
            var update = Builders<UserPostDocument>.Update.PullFilter(o => o.UserPosts, p => p.Id == id);

            return _repository.Collection.UpdateOneAsync(filter, update);
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return _repository.ExistsAsync(o => o.UserPosts.Any(p => p.Id == id));
        }

        public async Task<PagedResponse<Post>> BrowsePostsAsync(BrowseRequest request)
        {
            var filterDefinition = Builders<UserPostDocument>.Filter.Empty;

            if (request.UserId.HasValue)
            {
                filterDefinition &= Builders<UserPostDocument>.Filter.Eq(o => o.UserId, request.UserId.Value);
            }

            if (request.EventId.HasValue)
            {
                filterDefinition &= Builders<UserPostDocument>.Filter.ElemMatch(o => o.UserPosts, p => p.EventId == request.EventId.Value);
            }

            var sortDefinition = UserPostExtensions.ToSortDefinition(request.SortBy, request.Direction);

            var pagedPosts = await _repository.Collection.AggregateByPage<UserPostDocument>(filterDefinition, sortDefinition, request.PageNumber, request.PageSize);

            var posts = pagedPosts.data.SelectMany(o => o.UserPosts).ToList();

            return new PagedResponse<Post>(posts.Select(p => p.AsEntity()), request.PageNumber, request.PageSize, pagedPosts.totalElements);
        }

        public async Task<PagedResponse<Post>> BrowseUserPostsAsync(BrowseRequest request)
        {
            var filterDefinition = Builders<UserPostDocument>.Filter.Where(o => o.UserId == request.UserId);

            var sortDefinition = UserPostExtensions.ToSortDefinition(request.SortBy, request.Direction);

            var pagedEvents = await _repository.Collection.AggregateByPage<UserPostDocument>(filterDefinition, sortDefinition, request.PageNumber, request.PageSize);

            var posts = pagedEvents.data.SelectMany(o => o.UserPosts).ToList();

            return new PagedResponse<Post>(posts.Select(p => p.AsEntity()), request.PageNumber, request.PageSize, pagedEvents.totalElements);
        }

    }
}
