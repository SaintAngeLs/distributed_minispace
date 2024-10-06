using System.Collections;
using System.Collections.Generic;

namespace MiniSpace.Web.DTO.Wrappers
{
    public class PagedResponseDto<T> : ResponseDto<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public bool First { get; set; }
        public bool Last { get; set; }
        public bool Empty { get; set; }
        public int? NextPage => Page < TotalPages ? Page + 1 : (int?)null;
        public int? PreviousPage => Page > 1 ? Page - 1 : (int?)null;
    }

}