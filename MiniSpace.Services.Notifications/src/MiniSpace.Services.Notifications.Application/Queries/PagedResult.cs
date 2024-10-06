using Paralax.CQRS.Queries;

namespace MiniSpace.Services.Students.Application.Queries
{
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
            
            // Calculate if there's a next page
            int totalPages = (int)Math.Ceiling(total / (double)pageSize);
            NextPage = page < totalPages ? $"{baseUrl}?page={page + 1}&resultsPerPage={pageSize}" : null;
            PrevPage = page > 1 ? $"{baseUrl}?page={page - 1}&resultsPerPage={pageSize}" : null;
        }
    }

}
