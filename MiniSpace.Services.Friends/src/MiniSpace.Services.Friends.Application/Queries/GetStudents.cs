using Convey.CQRS.Queries;
using MiniSpace.Services.Friends.Application.Dto;

namespace MiniSpace.Services.Friends.Application.Queries
{
    public class GetStudents : IQuery<IEnumerable<StudentDto>>
    {
    }    
}
