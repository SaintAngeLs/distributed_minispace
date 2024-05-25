using System.Threading.Tasks;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents;
using MongoDB.Driver;

namespace MiniSpace.Services.Notifications.Infrastructure.Mongo.Repositories
{
    public interface IExtendedStudentNotificationsRepository : IStudentNotificationsRepository
    {
        Task<UpdateResult> BulkUpdateAsync(FilterDefinition<StudentNotificationsDocument> filter, UpdateDefinition<StudentNotificationsDocument> update);
    }
}
