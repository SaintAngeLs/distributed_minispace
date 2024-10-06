using Paralax.CQRS.Queries;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Core.Wrappers;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Posts.Application.Queries
{
    public class GetPosts : IQuery<PagedResponse<PostDto>>
    {
        public Guid? UserId { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid? EventId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "PublishDate";
        public string Direction { get; set; } = "asc";
    }
}
