using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Core.Wrappers;

namespace MiniSpace.Services.Events.Application.Queries
{
    public class GetSearchEvents : IQuery<PagedResponse<EventDto>>
    {
        public string Name { get; set; }
        public string Organizer { get; set; }
        public Guid? OrganizationId { get; set; }
        public string Category { get; set; }
        public string State { get; set; }
        public IEnumerable<Guid> Friends { get; set; }
        public string FriendsEngagementType { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public PageableDto Pageable { get; set; }
    }
}
