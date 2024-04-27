using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Application.Queries;
using MiniSpace.Services.Posts.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Queries.Handlers
{
    public class GetOrganizerPostsHandler : IQueryHandler<GetOrganizerPosts, IEnumerable<PostDto>>
    {
        private readonly IMongoRepository<PostDocument, Guid> _postRepository;

        public GetOrganizerPostsHandler(IMongoRepository<PostDocument, Guid> postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<PostDto>> HandleAsync(GetOrganizerPosts query, CancellationToken cancellationToken)
        {
            var documents = _postRepository.Collection.AsQueryable();
            documents = documents.Where(p => p.OrganizerId == query.OrganizerId);

            var posts = await documents.ToListAsync();
            return posts.Select(p => p.AsDto());
        }
    }
}