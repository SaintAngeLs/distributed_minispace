using System;
using System.Collections.Generic;
using MiniSpacePwa.DTO.Wrappers;

namespace MiniSpacePwa.Data.Reports
{
    public class SearchReports
    {
        public IEnumerable<string> ContextTypes { get; set; }
        public IEnumerable<string> States { get; set; }
        public Guid ReviewerId { get; set; }
        public PageableDto Pageable { get; set; }
        
        public SearchReports(IEnumerable<string> contextTypes, IEnumerable<string> states, Guid reviewerId, 
            PageableDto pageable)
        {
            ContextTypes = contextTypes;
            States = states;
            ReviewerId = reviewerId;
            Pageable = pageable;
        }
    }
}