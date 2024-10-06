using System;
using Astravent.Web.Wasm.DTO.Wrappers;

namespace Astravent.Web.Wasm.Models.Events
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
