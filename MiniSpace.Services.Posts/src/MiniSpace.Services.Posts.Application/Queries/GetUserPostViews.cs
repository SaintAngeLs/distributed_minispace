using System;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Application.DTO;
using MiniSpace.Services.Posts.Core.Wrappers;

namespace MiniSpace.Services.Posts.Application.Queries
{
    public class GetUserPostViews : IQuery<PagedResponse<ViewDto>>
    {
        public Guid UserId { get; }
        public int PageNumber { get; }
        public int PageSize { get; }

        public GetUserPostViews(Guid userId, int pageNumber = 1, int pageSize = 10)
        {
            UserId = userId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
