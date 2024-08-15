using System.Collections;
using System.Collections.Generic;

namespace MiniSpace.Web.DTO.Wrappers
{
    public class PagedResponseDto<T> : ResponseDto<T>
    {
         public IEnumerable<T> Items { get; }
        public int TotalPages { get; }
        public int TotalItems { get; }
        public int PageSize { get; }
        public int Page { get; }
        public bool First { get; }
        public bool Last { get; }
        public bool Empty { get; }
        public int? NextPage => Page < TotalPages ? Page + 1 : (int?)null;
        public int? PreviousPage => Page > 1 ? Page - 1 : (int?)null;

         public PagedResponseDto()
        {
            Items = new List<T>();
        }
    }
}