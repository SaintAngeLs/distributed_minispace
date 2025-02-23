using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Services.Comments.Application.Dto;

namespace MiniSpace.Services.Comments.Application.Services.Clients
{
    public interface IUsersServiceClient
    {
        Task<UserDto> GetAsync(Guid id);
    }
}