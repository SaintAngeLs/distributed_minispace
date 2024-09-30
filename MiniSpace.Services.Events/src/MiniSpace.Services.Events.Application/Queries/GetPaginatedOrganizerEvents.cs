using System;
using Paralax.CQRS.Queries;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Core.Wrappers;

namespace MiniSpace.Services.Events.Application.Queries
{
    public class GetPaginatedOrganizerEvents : IQuery<PagedResponse<EventDto>>
    {
        public Guid OrganizerId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
