using System;
using System.Threading.Tasks;
using MiniSpace.Services.Posts.Application.Dto;

namespace MiniSpace.Services.Posts.Application.Services.Clients
{
    public interface IUsersServiceClient
    {
        Task<UserEventsDto> GetAsync(Guid id);
        Task<UserDto> GetUserByIdAsync(Guid studentId); 
    }
}
