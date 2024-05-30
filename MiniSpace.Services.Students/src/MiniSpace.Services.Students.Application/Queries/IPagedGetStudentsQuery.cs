using Convey.CQRS.Queries;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public interface IPagedGetStudentsQuery : Convey.CQRS.Queries.IPagedQuery
    {
        new int Page { get; set; }
        new int ResultsPerPage { get; set; }
        new string OrderBy { get; set; }
        new string SortOrder { get; set; }
        int Results { get; set; }
    }
}
