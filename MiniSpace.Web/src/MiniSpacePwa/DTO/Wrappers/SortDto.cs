using System.Collections.Generic;

namespace MiniSpacePwa.DTO.Wrappers
{
    public class SortDto
    {
        public IEnumerable<string> SortBy { get; set; }
        public string Direction { get; set; }
    }
}