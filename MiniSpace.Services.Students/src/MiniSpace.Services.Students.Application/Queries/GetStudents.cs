using Paralax.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetStudents : IQuery<PagedResult<StudentDto>>, IPagedGetStudentsQuery
    {
        public int Page { get; set; }
        public int ResultsPerPage { get; set; }
        public string Name { get; set; }

        public string OrderBy { get; set; }
        public string SortOrder { get; set; }

        public int Results { get; set; }
    }    
}
