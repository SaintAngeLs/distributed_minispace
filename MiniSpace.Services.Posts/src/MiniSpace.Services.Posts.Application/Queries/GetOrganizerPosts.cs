using System.Collections;
using Convey.CQRS.Queries;
using MiniSpace.Services.Posts.Application.Dto;

namespace MiniSpace.Services.Posts.Application.Queries
{
    public class GetOrganizerPosts : IQuery<IEnumerable<PostDto>>
    {
        public Guid OrganizerId { get; set; }
    }
}