using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Comments.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class PageableDto
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public SortDto Sort { get; set; }
    }
}