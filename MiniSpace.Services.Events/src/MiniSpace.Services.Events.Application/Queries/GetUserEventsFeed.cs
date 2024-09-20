using System;
using System.Diagnostics.CodeAnalysis;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Core.Wrappers;

namespace MiniSpace.Services.Events.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetUserEventsFeed : IQuery<PagedResponse<EventDto>>
    {
        public Guid UserId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "PublishDate";
        public string Direction { get; set; } = "asc";

        public GetUserEventsFeed(Guid userId, int pageNumber = 1, int pageSize = 10, 
            string sortBy = "PublishDate", string direction = "asc")
        {
            UserId = userId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            SortBy = sortBy;
            Direction = direction;
        }
    }
}
