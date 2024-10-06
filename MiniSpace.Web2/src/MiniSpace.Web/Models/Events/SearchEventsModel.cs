using System;
using System.Collections.Generic;
using MiniSpace.Web.DTO.Wrappers;
using MiniSpace.Web.Models.Organizations;

namespace MiniSpace.Web.Models.Events
{
    public class SearchEventsModel
    {
        public string Name { get; set; }
        public string Organizer { get; set; }
        public OrganizationModel Organization { get; set; }
        public string Category { get; set; }
        public string State { get; set; }
        public HashSet<Guid> Friends { get; set; }
        public string FriendsEngagementType { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public PageableDto Pageable { get; set; }
    }
}
