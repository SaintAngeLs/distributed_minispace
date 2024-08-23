using System;
using System.Threading.Tasks;
using MiniSpace.Services.Posts.Application.Dto;

namespace MiniSpace.Services.Posts.Application.Services.Clients
{
    public interface IStudentsServiceClient
    {
        Task<StudentEventsDto> GetAsync(Guid id);
        Task<UserDto> GetStudentByIdAsync(Guid studentId); 
    }
}
