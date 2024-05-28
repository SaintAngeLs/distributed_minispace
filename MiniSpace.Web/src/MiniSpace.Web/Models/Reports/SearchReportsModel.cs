using System;
using System.Collections.Generic;
using MiniSpace.Web.DTO.Wrappers;

namespace MiniSpace.Web.Models.Reports
{
    public class SearchReportsModel
    {
        public HashSet<string> ContextTypes { get; set; }
        public HashSet<string> States { get; set; }
        public Guid ReviewerId { get; set; }
        public PageableDto Pageable { get; set; }
    }
}
