using System.Threading.Tasks;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents;
using MongoDB.Driver;

namespace MiniSpace.Services.Notifications.Infrastructure.Mongo.Repositories
{
    public interface IExtendedUserNotificationsRepository : IUserNotificationsRepository
    {
        Task<UpdateResult> BulkUpdateAsync(FilterDefinition<UserNotificationsDocument> filter, 
                                            UpdateDefinition<UserNotificationsDocument> update);
        Task<int> GetNotificationCount(Guid studentId);
        Task<List<NotificationDocument>> GetRecentNotifications(Guid studentId, int count);
    }
}
