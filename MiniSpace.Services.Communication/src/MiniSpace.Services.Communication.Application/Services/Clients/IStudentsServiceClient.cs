using System;
using System.Threading.Tasks;
using MiniSpace.Services.Communication.Application.Dto;

namespace MiniSpace.Services.Communication.Application.Services.Clients
{
    public interface IStudentsServiceClient
    {
        Task<UserDto> GetAsync(Guid id);
        public Task<IEnumerable<UserDto>> GetAllAsync();
        
    }
}