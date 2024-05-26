using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Comments.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class SortDto
    {
        public IEnumerable<string> SortBy { get; set; }
        public string Direction { get; set; }
    }
}