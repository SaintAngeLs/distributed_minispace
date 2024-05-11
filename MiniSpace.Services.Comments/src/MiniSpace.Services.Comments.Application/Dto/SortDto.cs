using System.Collections.Generic;

namespace MiniSpace.Services.Comments.Application.Dto
{
    public class SortDto
    {
        public IEnumerable<string> SortBy { get; set; }
        public string Direction { get; set; }
    }
}