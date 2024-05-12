using Convey.CQRS.Queries;
using MiniSpace.Services.Notifications.Application.Dto;

namespace MiniSpace.Services.Notifications.Application.Queries
{
    public class GetStudentEvents: IQuery<StudentEventsDto>
    {
        public Guid StudentId { get; set; }
    }
}