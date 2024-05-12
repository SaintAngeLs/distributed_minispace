using Convey.CQRS.Queries;
using MiniSpace.Services.Posts.Application.Dto;

namespace MiniSpace.Services.Posts.Application.Queries
{
    public class GetPost : IQuery<PostDto>
    {
        public Guid PostId { get; set; }
    }
}
