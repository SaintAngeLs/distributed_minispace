using System;
using MiniSpace.Web.DTO.Wrappers;

namespace MiniSpace.Web.Data.Events
{
    public class SearchEvents
    {
        public string Name { get; set; }
        public string Organizer { get; set; }
        public string Category { get; set; }
        public string State { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public PageableDto Pageable { get; set; }
        
        public SearchEvents(string name, string organizer, string category, string state,
            string dateFrom, string dateTo, PageableDto pageable)
        {
            Name = name;
            Organizer = organizer;
            Category = category;
            State = state;
            DateFrom = dateFrom;
            DateTo = dateTo;
            Pageable = pageable;
        }
    }
}