using Convey.CQRS.Queries;
using MiniSpace.Services.Notifications.Application.Dto;

namespace MiniSpace.Services.Notifications.Application.Queries
{
    public class GetStudent : IQuery<StudentDto>
    {
        public Guid StudentId { get; set; }
    }    
}
