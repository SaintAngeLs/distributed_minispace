using Paralax.CQRS.Queries;

namespace MiniSpace.Services.Email.Application.Queries
{
    public interface IPagedQuery
    {
        int Page { get; set; }
        int ResultsPerPage { get; set; }
        string OrderBy { get; set; }
        string SortOrder { get; set; }
    }
}
