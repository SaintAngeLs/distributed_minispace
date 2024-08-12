using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Events.Application.DTO
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalItems { get; }
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((decimal)TotalItems / PageSize) : 0;

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
