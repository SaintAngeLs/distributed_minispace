using System.Diagnostics.CodeAnalysis;
using Paralax.CQRS.Queries;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Comments.Application.Dto;
using MiniSpace.Services.Comments.Application.Queries;
using MiniSpace.Services.Comments.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetCommentHandler : IQueryHandler<GetComment, CommentDto>
    {
        private readonly IMongoRepository<CommentDocument, Guid> _repository;
        
        public GetCommentHandler(IMongoRepository<CommentDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<CommentDto> HandleAsync(GetComment query, CancellationToken cancellationToken)
        {
            var comment = await _repository.GetAsync(query.CommentId);
            return comment?.AsDto();
        }
    }    
}
