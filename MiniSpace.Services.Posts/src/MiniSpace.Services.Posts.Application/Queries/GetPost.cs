using System.Diagnostics.CodeAnalysis;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Posts.Application.Dto;

namespace MiniSpace.Services.Posts.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetPost : IQuery<PostDto>
    {
        public Guid PostId { get; set; }
    }
}
