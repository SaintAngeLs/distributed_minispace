using Convey.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;

namespace MiniSpace.Services.Students.Application.Queries
{
    public class GetStudent : IQuery<StudentDto>
    {
        public Guid StudentId { get; set; }
    }    
}
