using Convey.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;

namespace MiniSpace.Services.Students.Application.Queries
{
    public class GetStudents : IQuery<IEnumerable<StudentDto>>, IPagedGetStudentsQuery
    {
        public int Page { get; set; } = 1;
        public int ResultsPerPage { get; set; } = 10;

        public string OrderBy { get; set; } = "LastName"; 
        public string SortOrder { get; set; } = "asc";

        public int Results { get; set; } = 10;
    }    
}
