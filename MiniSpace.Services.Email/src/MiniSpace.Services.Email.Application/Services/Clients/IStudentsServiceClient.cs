using System;
using System.Threading.Tasks;
using MiniSpace.Services.Email.Application.Dto;

namespace MiniSpace.Services.Email.Application.Services.Clients
{
    public interface IStudentsServiceClient
    {
        Task<StudentDto> GetAsync(Guid id);
        public Task<IEnumerable<StudentDto>> GetAllAsync();
    }
}