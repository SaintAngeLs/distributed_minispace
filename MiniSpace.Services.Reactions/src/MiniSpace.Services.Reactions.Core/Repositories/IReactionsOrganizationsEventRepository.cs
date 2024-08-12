using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Core.Repositories
{
    public interface IReactionsOrganizationsEventRepository
    {
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(Event @event);
    }
}