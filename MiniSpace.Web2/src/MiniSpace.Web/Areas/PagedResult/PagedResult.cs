using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Web.Areas.PagedResult
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalItems { get; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);

        public int? NextPage => Page < TotalPages ? Page + 1 : (int?)null;
        public int? PreviousPage => Page > 1 ? Page - 1 : (int?)null;

        public PagedResult(IEnumerable<T> items, int page, int pageSize, int totalItems)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalItems = totalItems;
        }
    }
}