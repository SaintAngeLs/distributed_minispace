using Convey.CQRS.Queries;
using MiniSpace.Services.MediaFiles.Application.Dto;

namespace MiniSpace.Services.MediaFiles.Application.Queries
{
    public class GetStudentEvents: IQuery<StudentEventsDto>
    {
        public Guid StudentId { get; set; }
    }
}