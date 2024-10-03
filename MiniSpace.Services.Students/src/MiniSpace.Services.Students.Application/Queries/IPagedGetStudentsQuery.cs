using Paralax.CQRS.Queries;

namespace MiniSpace.Services.Students.Application.Queries
{
    public interface IPagedGetStudentsQuery : Paralax.CQRS.Queries.IPagedQuery
    {
        new int Page { get; set; }
        new int ResultsPerPage { get; set; }
        new string Name { get; set; }
        new string OrderBy { get; set; }
        new string SortOrder { get; set; }
        new int Results { get; set; }
    }
}
