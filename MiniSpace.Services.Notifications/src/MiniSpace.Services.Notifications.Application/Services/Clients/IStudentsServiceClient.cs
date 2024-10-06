using System;
using System.Threading.Tasks;
using MiniSpace.Services.Notifications.Application.Dto;

namespace MiniSpace.Services.Notifications.Application.Services.Clients
{
    public interface IStudentsServiceClient
    {
        Task<UserDto> GetAsync(Guid id);
        public Task<IEnumerable<UserDto>> GetAllAsync();
        
    }
}