using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Application.Queries;
using MiniSpace.Services.Posts.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Posts.Infrastructure.Mongo.Queries.Handlers
{
    public class GetPostHandler : IQueryHandler<GetPost, PostDto>
    {
        private readonly IMongoRepository<PostDocument, Guid> _repository;

        public GetPostHandler(IMongoRepository<PostDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<PostDto> HandleAsync(GetPost query, CancellationToken cancellationToken)
        {
            var post = await _repository.GetAsync(query.PostId);

            return post?.AsDto();
        }
    }
}
