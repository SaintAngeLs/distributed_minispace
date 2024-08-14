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
    public class OrganizationPostMongoRepository : IOrganizationPostRepository
    {
        private readonly IMongoRepository<OrganizationPostDocument, Guid> _repository;

        public OrganizationPostMongoRepository(IMongoRepository<OrganizationPostDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Post> GetAsync(Guid id)
        {
            var organizationPost = await _repository.GetAsync(e => e.Id == id);
            return organizationPost?.OrganizationPosts.FirstOrDefault(p => p.Id == id)?.AsEntity();
        }

        public async Task<IEnumerable<Post>> GetToUpdateAsync()
        {
            var postsToUpdate = await _repository.Collection.AsQueryable()
                .SelectMany(o => o.OrganizationPosts)
                .Where(e => e.State == State.ToBePublished || e.State == State.Published)
                .ToListAsync();

            return postsToUpdate.Select(p => p.AsEntity());
        }

        public async Task<IEnumerable<Post>> GetByOrganizationIdAsync(Guid organizationId)
        {
            var posts = await _repository.Collection.AsQueryable()
                .Where(o => o.OrganizationId == organizationId)
                .SelectMany(o => o.OrganizationPosts)
                .ToListAsync();

            return posts.Select(p => p.AsEntity());
        }

        public Task AddAsync(Post post)
        {
            var organizationPost = new OrganizationPost(post.Id, post.OrganizationId.Value, new List<Post> { post });
            return _repository.AddAsync(organizationPost.AsDocument());
        }

        public Task UpdateAsync(Post post)
        {
            var filter = Builders<OrganizationPostDocument>.Filter.And(
                Builders<OrganizationPostDocument>.Filter.Eq(o => o.OrganizationId, post.OrganizationId),
                Builders<OrganizationPostDocument>.Filter.ElemMatch(o => o.OrganizationPosts, p => p.Id == post.Id));

            var update = Builders<OrganizationPostDocument>.Update.Set(o => o.OrganizationPosts[-1], post.AsDocument());

            return _repository.Collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(Guid id)
        {
            var filter = Builders<OrganizationPostDocument>.Filter.ElemMatch(o => o.OrganizationPosts, p => p.Id == id);
            var update = Builders<OrganizationPostDocument>.Update.PullFilter(o => o.OrganizationPosts, p => p.Id == id);

            return _repository.Collection.UpdateOneAsync(filter, update);
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return _repository.ExistsAsync(o => o.OrganizationPosts.Any(p => p.Id == id));
        }

        public async Task<IEnumerable<Post>> GetByEventIdAsync(Guid eventId)
        {
            var posts = await _repository.Collection.AsQueryable()
                .Where(o => o.OrganizationPosts.Any(p => p.EventId == eventId))
                .SelectMany(o => o.OrganizationPosts.Where(p => p.EventId == eventId))
                .ToListAsync();

            return posts.Select(p => p.AsEntity());
        }

        public async Task<PagedResponse<Post>> BrowseOrganizationPostsAsync(BrowseRequest request)
        {
            var filterDefinition = Builders<OrganizationPostDocument>.Filter.Where(o => o.OrganizationId == request.OrganizationId);

            var sortDefinition = Extensions.ToSortDefinition(request.SortBy, request.Direction);

            var pagedEvents = await _repository.Collection.AggregateByPage(filterDefinition, sortDefinition, request.PageNumber, request.PageSize);

            var posts = pagedEvents.data.SelectMany(o => o.OrganizationPosts).ToList();

            return new PagedResponse<Post>(posts.Select(p => p.AsEntity()), request.PageNumber, request.PageSize, pagedEvents.totalElements);
        }
    }
}
