using Convey.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;

namespace MiniSpace.Services.Students.Application.Queries
{
    public class GetStudentEvents: IQuery<StudentEventsDto>
    {
        public Guid StudentId { get; set; }
    }
}