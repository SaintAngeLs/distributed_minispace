using System.Collections.Generic;

namespace Astravent.Web.Wasm.DTO.Wrappers
{
    public class SortDto
    {
        public IEnumerable<string> SortBy { get; set; }
        public string Direction { get; set; }
    }
}