using System;
using System.Collections.Generic;
using Astravent.Web.Wasm.DTO.Wrappers;

namespace Astravent.Web.Wasm.Data.Events
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
        
        public SearchEvents(string name, string organizer, Guid organizationId, Guid rootOrganizationId,
            string category, string state, IEnumerable<Guid> friends, string friendsEngagementType,
            string dateFrom, string dateTo, PageableDto pageable)
        {
            Name = name;
            Organizer = organizer;
            OrganizationId = organizationId;
            RootOrganizationId = rootOrganizationId;
            Category = category;
            State = state;
            Friends = friends;
            FriendsEngagementType = friendsEngagementType;
            DateFrom = dateFrom;
            DateTo = dateTo;
            Pageable = pageable;
        }
    }
}