using MiniSpace.Services.Notifications.Core.Entities;
using System;
using System.Threading.Tasks;

namespace MiniSpace.Services.Notifications.Core.Repositories
{
    public interface IFriendEventRepository
    {
        Task<FriendEvent> GetAsync(Guid id);
        Task AddAsync(FriendEvent friendEvent);
        Task UpdateAsync(FriendEvent friendEvent);
        Task DeleteAsync(Guid id);
    }
}
