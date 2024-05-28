using System;
using System.Collections.Generic;
using MiniSpace.Web.DTO.Wrappers;

namespace MiniSpace.Web.Models.Reports
{
    public class SearchReportsModel
    {
        public IEnumerable<string> ContextTypes { get; set; }
        public IEnumerable<string> States { get; set; }
        public bool OnlyReviewedByYou { get; set; }
        public PageableDto Pageable { get; set; }
    }
}
