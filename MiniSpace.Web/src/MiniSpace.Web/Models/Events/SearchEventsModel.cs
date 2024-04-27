using System;
using MiniSpace.Web.DTO.Wrappers;

namespace MiniSpace.Web.Models.Events
{
    public class SearchEventsModel
    {
        public string Name { get; set; }
        public string Organizer { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public PageableDto Pageable { get; set; }
    }
}
