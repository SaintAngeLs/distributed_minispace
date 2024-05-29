using Convey.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;

namespace MiniSpace.Services.Students.Application.Queries
{
    public class GetStudents : IQuery<PagedResult<StudentDto>>, IPagedGetStudentsQuery
    {
        public int Page { get; set; }
        public int ResultsPerPage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string OrderBy { get; set; }
        public string SortOrder { get; set; }

        public int Results { get; set; }
    }    
}
