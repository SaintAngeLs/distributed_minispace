using Paralax.CQRS.Queries;
using MiniSpace.Services.Reports.Application.DTO;
using MiniSpace.Services.Reports.Core.Wrappers;
using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Reports.Application.Queries
{
    public class SearchReports : IQuery<PagedResponse<ReportDto>>
    {
        public IEnumerable<string> ContextTypes { get; set; }
        public IEnumerable<string> States { get; set; }
        public Guid ReviewerId { get; set; }

        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10; 
        public string SortBy { get; set; } 
        public string Direction { get; set; } 
    }
}
