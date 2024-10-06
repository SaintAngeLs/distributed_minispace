using Paralax.CQRS.Queries;
using System.Collections.Generic;

namespace MiniSpace.Services.Notifications.Application.Queries
{
    public interface IPagedNotificationsQuery : IPagedQuery
    {
        new int Page { get; set; }
        new int ResultsPerPage { get; set; }
        new string OrderBy { get; set; }
        new string SortOrder { get; set; }
    }

    public class PagedResult<T>
    {
        public List<T> Results { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string NextPage { get; set; }
        public string PrevPage { get; set; }

        public PagedResult(List<T> results, int total, int pageSize, int page, string baseUrl)
        {
            Results = results;
            Total = total;
            PageSize = pageSize;
            Page = page;

            int totalPages = (int)Math.Ceiling(total / (double)pageSize);
            NextPage = page < totalPages ? $"{baseUrl}?page={page + 1}&resultsPerPage={pageSize}" : null;
            PrevPage = page > 1 ? $"{baseUrl}?page={page - 1}&resultsPerPage={pageSize}" : null;
        }
    }
}
