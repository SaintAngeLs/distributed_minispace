using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Comments.Application.Dto;
using MiniSpace.Services.Comments.Application.Queries;
using MiniSpace.Services.Comments.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Queries.Handlers
{
    public class GetCommentHandler : IQueryHandler<Get>
    {
    
    }    
}
