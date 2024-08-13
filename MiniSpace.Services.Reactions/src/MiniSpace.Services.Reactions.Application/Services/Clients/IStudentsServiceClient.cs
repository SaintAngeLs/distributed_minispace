using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Services.Reactions.Application.Dto;

namespace MiniSpace.Services.Reactions.Application.Services.Clients
{
    public interface IStudentsServiceClient
    {
        Task<bool> StudentExistsAsync(Guid id);
    }
}