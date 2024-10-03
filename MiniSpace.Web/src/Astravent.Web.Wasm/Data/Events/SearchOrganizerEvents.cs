using System;
using Astravent.Web.Wasm.DTO.Wrappers;

namespace Astravent.Web.Wasm.Data.Events
{
    public class SearchOrganizerEvents
    {
        public string Name { get; set; }
        public Guid OrganizerId { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string State { get; set; }
        public PageableDto Pageable { get; set; }
        
        public SearchOrganizerEvents(string name, Guid organizerId, string dateFrom, string dateTo,
            string state, PageableDto pageable)
        {
            Name = name;
            OrganizerId = organizerId;
            DateFrom = dateFrom;
            DateTo = dateTo;
            State = state;
            Pageable = pageable;
        }
    }
}
