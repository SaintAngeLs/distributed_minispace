using Paralax.CQRS.Queries;
using MiniSpace.Services.Reports.Application.DTO;
using MiniSpace.Services.Reports.Core.Wrappers;
using System;

namespace MiniSpace.Services.Reports.Application.Queries
{
    public class GetUserReports : IQuery<PagedResponse<ReportDto>>
    {
        public Guid UserId { get; set; }        
        public int Page { get; set; } = 1;      
        public int Results { get; set; } = 10;  
        public string SortBy { get; set; }      
        public string Direction { get; set; }   
        public IEnumerable<string> ContextTypes { get; set; } 
        public IEnumerable<string> States { get; set; }       
    }
}
