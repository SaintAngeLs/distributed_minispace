using System.Diagnostics.CodeAnalysis;
using Paralax.CQRS.Queries;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Posts.Application;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Application.Queries;
using MiniSpace.Services.Posts.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetOrganizerPostsHandler : IQueryHandler<GetOrganizerPosts, IEnumerable<PostDto>>
    {
        private readonly IMongoRepository<PostDocument, Guid> _postRepository;
        private readonly IAppContext _appContext;

        public GetOrganizerPostsHandler(IMongoRepository<PostDocument, Guid> postRepository, IAppContext appContext)
        {
            _postRepository = postRepository;
            _appContext = appContext;
        }

        public async Task<IEnumerable<PostDto>> HandleAsync(GetOrganizerPosts query, CancellationToken cancellationToken)
        {
            var identity = _appContext.Identity;
            if (!identity.IsAuthenticated && identity.Id != query.OrganizerId && !identity.IsAdmin)
            {
                return Enumerable.Empty<PostDto>();
            }
            
            var documents = _postRepository.Collection.AsQueryable();
            documents = documents.Where(p => p.UserId == query.OrganizerId);

            var posts = await documents.ToListAsync();
            return posts.Select(p => p.AsDto());
        }
    }
}