using System;
using MiniSpace.Web.DTO.Wrappers;

namespace MiniSpace.Web.Models.Events
{
    public class SearchOrganizerEventsModel
    {
        public Guid OrganizerId { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public PageableDto Pageable { get; set; }
    }
}
