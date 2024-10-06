using System;
using System.Collections.Generic;
using Astravent.Web.Wasm.DTO.Wrappers;

namespace Astravent.Web.Wasm.Models.Reports
{
    public class SearchReportsModel
    {
        public IEnumerable<string> ContextTypes { get; set; }
        public IEnumerable<string> States { get; set; }
        public bool OnlyReviewedByYou { get; set; }
        public PageableDto Pageable { get; set; }

        public SearchReportsModel()
        {
            SetDefaultValues();
        }
        
        public void SetDefaultValues()
        {
            ContextTypes = ["Event", "Post", "Comment", "StudentProfile"];
            States = ["Submitted", "UnderReview", "Resolved", "Rejected", "Cancelled"];
            OnlyReviewedByYou = false;
            Pageable = new PageableDto()
            {
                Page = 1,
                Size = 5,
                Sort = new SortDto()
                {
                    SortBy = new List<string>() { "updatedAt", "createdAt" },
                    Direction = "des"
                }
            };
        }
    }
}
