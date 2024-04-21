using System;
using MiniSpace.Web.DTO.Wrappers;

namespace MiniSpace.Web.DTO.Data.Events
{
    public class SearchEvents
    {
        public string Name { get; set; }
        public string Organizer { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public PageableDto Pageable { get; set; }
        
        public SearchEvents(string name, string organizer, DateTime dateFrom, DateTime dateTo, PageableDto pageable)
        {
            Name = name;
            Organizer = organizer;
            DateFrom = dateFrom;
            DateTo = dateTo;
            Pageable = pageable;
        }
    }
}