using System.Diagnostics.CodeAnalysis;
using Convey.CQRS.Queries;
using MiniSpace.Services.Posts.Application.Dto;

namespace MiniSpace.Services.Posts.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetPosts : IQuery<IEnumerable<PostDto>>
    {
        public Guid EventId { get; set; }
    }
}
