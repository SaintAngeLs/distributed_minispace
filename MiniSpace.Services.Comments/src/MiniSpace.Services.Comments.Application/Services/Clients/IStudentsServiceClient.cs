using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Services.Comments.Application.Dto;

namespace MiniSpace.Services.Comments.Application.Services.Clients
{
    public interface IStudentsServiceClient
    {
        Task<UserDto> GetAsync(Guid id);
    }
}