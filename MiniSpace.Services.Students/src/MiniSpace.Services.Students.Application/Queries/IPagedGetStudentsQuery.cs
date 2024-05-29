using Convey.CQRS.Queries;

namespace MiniSpace.Services.Students.Application.Queries
{
    public interface IPagedGetStudentsQuery : Convey.CQRS.Queries.IPagedQuery
    {
        new int Page { get; set; }
        new int ResultsPerPage { get; set; }
        new string FirstName { get; set; }
        new string LastName { get; set; }
        new string OrderBy { get; set; }
        new string SortOrder { get; set; }
        int Results { get; set; }
    }
}
