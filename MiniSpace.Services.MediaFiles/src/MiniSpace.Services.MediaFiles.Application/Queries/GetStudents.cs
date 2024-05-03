using Convey.CQRS.Queries;
using MiniSpace.Services.MediaFiles.Application.Dto;

namespace MiniSpace.Services.MediaFiles.Application.Queries
{
    public class GetStudents : IQuery<IEnumerable<StudentDto>>
    {
    }    
}
