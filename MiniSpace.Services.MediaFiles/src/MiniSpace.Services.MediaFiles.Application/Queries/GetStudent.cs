using Convey.CQRS.Queries;
using MiniSpace.Services.MediaFiles.Application.Dto;

namespace MiniSpace.Services.MediaFiles.Application.Queries
{
    public class GetStudent : IQuery<StudentDto>
    {
        public Guid StudentId { get; set; }
    }    
}
