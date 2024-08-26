using System.Threading.Tasks;
using MiniSpace.Services.Communication.Core.Repositories;
using MiniSpace.Services.Communication.Infrastructure.Mongo.Documents;
using MongoDB.Driver;

namespace MiniSpace.Services.Communication.Infrastructure.Mongo.Repositories
{
    public interface IExtendedStudentNotificationsRepository : IStudentNotificationsRepository
    {
        Task<UpdateResult> BulkUpdateAsync(FilterDefinition<StudentNotificationsDocument> filter, UpdateDefinition<StudentNotificationsDocument> update);
        Task<int> GetNotificationCount(Guid studentId);
        Task<List<NotificationDocument>> GetRecentNotifications(Guid studentId, int count);
    }
}
