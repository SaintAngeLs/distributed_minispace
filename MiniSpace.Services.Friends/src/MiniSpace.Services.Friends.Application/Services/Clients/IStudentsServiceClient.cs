using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Services.Friends.Application.Dto;

namespace MiniSpace.Services.Friends.Application.Services.Clients
{
    public interface IStudentsServiceClient
    {
        Task<StudentDto> GetAsync(Guid id);
        public Task<IEnumerable<StudentDto>> GetAllAsync();
    }
}