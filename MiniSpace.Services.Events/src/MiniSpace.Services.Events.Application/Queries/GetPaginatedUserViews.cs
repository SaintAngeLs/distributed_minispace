using System;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Core.Wrappers;

namespace MiniSpace.Services.Events.Application.Queries
{
    public class GetPaginatedUserViews : IQuery<PagedResponse<ViewDto>>
    {
        public Guid UserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
