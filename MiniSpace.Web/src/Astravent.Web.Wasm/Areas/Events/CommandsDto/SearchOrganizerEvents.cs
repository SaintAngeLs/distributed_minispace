using System;
using Astravent.Web.Wasm.DTO.Wrappers;

namespace Astravent.Web.Wasm.Areas.Events.CommandsDto
{
    public class SearchOrganizerEvents
    {
        public Guid OrganizerId { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public PageableDto Pageable { get; set; }
    }
}
