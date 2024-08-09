using System;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application.DTO;

namespace MiniSpace.Services.Events.Application.Queries
{
    public class GetPaginatedOrganizerEvents : IQuery<MiniSpace.Services.Events.Application.DTO.PagedResult<EventDto>>
    {
        public Guid OrganizerId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
