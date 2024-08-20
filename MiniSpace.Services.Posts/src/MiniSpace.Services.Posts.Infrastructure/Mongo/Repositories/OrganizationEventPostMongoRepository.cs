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
    public class OrganizationEventPostMongoRepository : IOrganizationEventPostRepository
    {
        private readonly IMongoRepository<OrganizationEventPostDocument, Guid> _repository;

        public OrganizationEventPostMongoRepository(IMongoRepository<OrganizationEventPostDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Post> GetAsync(Guid id)
        {
            var organizationEventPost = await _repository.GetAsync(e => e.Id == id);
            return organizationEventPost?.EventPosts.FirstOrDefault(p => p.Id == id)?.AsEntity();
        }

        public async Task<IEnumerable<Post>> GetToUpdateAsync()
        {
            var postsToUpdate = await _repository.Collection.AsQueryable()
                .SelectMany(o => o.EventPosts)
                .Where(e => e.State == State.ToBePublished || e.State == State.Published)
                .ToListAsync();

            return postsToUpdate.Select(p => p.AsEntity());
        }

        public async Task<IEnumerable<Post>> GetByEventIdAsync(Guid eventId)
        {
            var posts = await _repository.Collection.AsQueryable()
                .Where(o => o.EventPosts.Any(p => p.EventId == eventId))
                .SelectMany(o => o.EventPosts.Where(p => p.EventId == eventId))
                .ToListAsync();

            return posts.Select(p => p.AsEntity());
        }

        public async Task<IEnumerable<Post>> GetByOrganizationEventIdAsync(Guid organizationId, Guid eventId)
        {
            var posts = await _repository.Collection.AsQueryable()
                .Where(o => o.OrganizationId == organizationId && o.EventPosts.Any(p => p.EventId == eventId))
                .SelectMany(o => o.EventPosts.Where(p => p.EventId == eventId))
                .ToListAsync();

            return posts.Select(p => p.AsEntity());
        }

        public Task AddAsync(Post post)
        {
            var organizationEventPost = new OrganizationEventPost(post.Id, post.OrganizationId.Value, new List<Post> { post });
            return _repository.AddAsync(organizationEventPost.AsDocument());
        }

        public Task UpdateAsync(Post post)
        {
            var filter = Builders<OrganizationEventPostDocument>.Filter.And(
                Builders<OrganizationEventPostDocument>.Filter.Eq(o => o.OrganizationId, post.OrganizationId),
                Builders<OrganizationEventPostDocument>.Filter.ElemMatch(o => o.EventPosts, p => p.Id == post.Id));

            var update = Builders<OrganizationEventPostDocument>.Update.Set("EventPosts.$", post.AsDocument());

            return _repository.Collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(Guid id)
        {
            var filter = Builders<OrganizationEventPostDocument>.Filter.ElemMatch(o => o.EventPosts, p => p.Id == id);
            var update = Builders<OrganizationEventPostDocument>.Update.PullFilter(o => o.EventPosts, p => p.Id == id);

            return _repository.Collection.UpdateOneAsync(filter, update);
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return _repository.ExistsAsync(o => o.EventPosts.Any(p => p.Id == id));
        }

        public async Task<PagedResponse<Post>> BrowsePostsAsync(BrowseRequest request)
        {
            var filterDefinition = Builders<OrganizationEventPostDocument>.Filter.Empty;

            if (request.OrganizationId.HasValue)
            {
                filterDefinition &= Builders<OrganizationEventPostDocument>.Filter.Eq(o => o.OrganizationId, request.OrganizationId.Value);
            }

            if (request.EventId.HasValue)
            {
                filterDefinition &= Builders<OrganizationEventPostDocument>.Filter.ElemMatch(o => o.EventPosts, p => p.EventId == request.EventId.Value);
            }

            var sortDefinition = OrganizationEventPostExtensions.ToSortDefinition(request.SortBy, request.Direction);

            var pagedEvents = await _repository.Collection.AggregateByPage(filterDefinition, sortDefinition, request.PageNumber, request.PageSize);

            var posts = pagedEvents.data.SelectMany(o => o.EventPosts.Where(p => p.EventId == request.EventId)).ToList();

            return new PagedResponse<Post>(posts.Select(p => p.AsEntity()), request.PageNumber, request.PageSize, pagedEvents.totalElements);
        }

        public async Task<PagedResponse<Post>> BrowseOrganizationEventPostsAsync(BrowseRequest request)
        {
            var filterDefinition = Builders<OrganizationEventPostDocument>.Filter.Empty;

            if (request.OrganizationId.HasValue)
            {
                filterDefinition &= Builders<OrganizationEventPostDocument>.Filter.Eq(o => o.OrganizationId, request.OrganizationId.Value);
            }

            if (request.EventId.HasValue)
            {
                filterDefinition &= Builders<OrganizationEventPostDocument>.Filter.ElemMatch(o => o.EventPosts, p => p.EventId == request.EventId.Value);
            }

            var sortDefinition = OrganizationEventPostExtensions.ToSortDefinition(request.SortBy, request.Direction);

            var pagedEvents = await _repository.Collection.AggregateByPage<OrganizationEventPostDocument>(filterDefinition, sortDefinition, request.PageNumber, request.PageSize);

            var posts = pagedEvents.data.SelectMany(o => o.EventPosts.Where(p => p.EventId == request.EventId)).ToList();

            return new PagedResponse<Post>(posts.Select(p => p.AsEntity()), request.PageNumber, request.PageSize, pagedEvents.totalElements);
        }
    }
}
