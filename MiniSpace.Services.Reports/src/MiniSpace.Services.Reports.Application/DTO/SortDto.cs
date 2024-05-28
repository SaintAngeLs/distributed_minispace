using System.Collections.Generic;

namespace MiniSpace.Services.Reports.Application.DTO
{
    public class SortDto
    {
        public IEnumerable<string> SortBy { get; set; }
        public string Direction { get; set; }
    }
}