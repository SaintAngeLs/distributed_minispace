namespace MiniSpace.Web.DTO.Wrappers
{
    public class PagedResponseDto<T> : ResponseDto<T>
    {
        public int TotalPages { get; set; }
        public int TotalElements { get; set; }
        public int Size { get; set; }
        public int Number { get; set; }
        public bool First { get; set; }
        public bool Last { get; set; }
        public bool Empty { get; set; }
    }
}