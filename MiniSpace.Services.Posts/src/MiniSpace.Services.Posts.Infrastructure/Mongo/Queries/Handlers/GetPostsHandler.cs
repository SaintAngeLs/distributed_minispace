using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Application.Queries;
using MiniSpace.Services.Posts.Core.Entities;
using MiniSpace.Services.Posts.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Queries.Handlers
{
    public class GetPostsHandler : IQueryHandler<GetPosts, IEnumerable<PostDto>>
    {
        private readonly IMongoRepository<PostDocument, Guid> _postRepository;

        public GetPostsHandler(IMongoRepository<PostDocument, Guid> postRepository)
        {
            _postRepository = postRepository;
        }
        
        public async Task<IEnumerable<PostDto>> HandleAsync(GetPosts query, CancellationToken cancellationToken)
        {
            var documents = _postRepository.Collection.AsQueryable();
            documents = documents.Where(p => p.EventId == query.EventId && p.State == State.Published);

            var posts = await documents.ToListAsync();
            return posts.Select(p => p.AsDto());
        }
    }    
}
