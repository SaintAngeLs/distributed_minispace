using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Reactions.Application.Dto;
using MiniSpace.Services.Reactions.Application.Queries;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Queries.Handlers
{
    // public class GetPostsHandler : IQueryHandler<GetPosts, IEnumerable<PostDto>>
    // {
    //     private readonly IMongoRepository<PostDocument, Guid> _postRepository;

    //     public GetPostsHandler(IMongoRepository<PostDocument, Guid> postRepository)
    //     {
    //         _postRepository = postRepository;
    //     }
        
    //     public async Task<IEnumerable<PostDto>> HandleAsync(GetPosts query, CancellationToken cancellationToken)
    //     {
    //         var documents = _postRepository.Collection.AsQueryable();
    //         documents = documents.Where(p => p.EventId == query.EventId);

    //         var posts = await documents.ToListAsync();
    //         return posts.Select(p => p.AsDto());
    //     }
    // }    
}
