using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application.DTO;
using System.Collections.Generic;

namespace MiniSpace.Services.Events.Application.Queries
{
    public class GetPaginatedEvents : IQuery<MiniSpace.Services.Events.Application.DTO.PagedResult<EventDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}