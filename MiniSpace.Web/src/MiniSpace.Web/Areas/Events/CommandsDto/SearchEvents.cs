using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Web.DTO.Wrappers;

namespace MiniSpace.Web.Areas.Events.CommandsDto
{
    public class SearchEvents
    {
        public string Name { get; set; }
        public string Organizer { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid RootOrganizationId { get; set; }
        public string Category { get; set; }
        public string State { get; set; }
        public IEnumerable<Guid> Friends { get; set; }
        public string FriendsEngagementType { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public PageableDto Pageable { get; set; }
    }
}