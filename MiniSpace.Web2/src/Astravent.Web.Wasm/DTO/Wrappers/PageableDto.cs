namespace Astravent.Web.Wasm.DTO.Wrappers
{
    public class PageableDto
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public SortDto Sort { get; set; }
    }
}