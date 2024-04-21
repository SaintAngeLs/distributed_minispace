using Convey.CQRS.Queries;
using MiniSpace.Services.Friends.Application.Dto;

namespace MiniSpace.Services.Friends.Application.Queries
{
    public class GetStudent : IQuery<StudentDto>
    {
        public Guid StudentId { get; set; }
    }    
}
