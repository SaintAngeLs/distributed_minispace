using System;
using System.Collections.Generic;
using Astravent.Web.Wasm.DTO.Wrappers;
using Astravent.Web.Wasm.Models.Organizations;

namespace Astravent.Web.Wasm.Models.Events
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
