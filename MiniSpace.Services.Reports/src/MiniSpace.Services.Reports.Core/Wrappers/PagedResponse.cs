using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Reports.Core.Wrappers
{
    public class PagedResponse<T>
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

        public PagedResponse(IEnumerable<T> items, int page, int pageSize, int totalItems)
        {
            Items = items;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPages = pageSize > 0 ? (int)Math.Ceiling((decimal)totalItems / pageSize) : 0;
            Page = page;
            First = page == 1;
            Last = page == TotalPages;
            Empty = !items.Any();
        }
    }
}
