using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Posts.Application.Dto;

namespace MiniSpace.Services.Posts.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetOrganizerPosts : IQuery<IEnumerable<PostDto>>
    {
        public Guid OrganizerId { get; set; }
    }
}