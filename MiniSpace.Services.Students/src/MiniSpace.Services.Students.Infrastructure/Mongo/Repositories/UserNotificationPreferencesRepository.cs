using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Repositories
{
    [ExcludeFromCodeCoverage]
    public class UserNotificationPreferencesRepository : IUserNotificationPreferencesRepository
    {
        private readonly IMongoRepository<UserNotificationsDocument, Guid> _repository;

        public UserNotificationPreferencesRepository(IMongoRepository<UserNotificationsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<NotificationPreferences> GetNotificationPreferencesAsync(Guid studentId)
        {
            var userNotificationsDocument = await _repository.GetAsync(x => x.StudentId == studentId);
            return userNotificationsDocument?.NotificationPreferences;
        }

        public async Task UpdateNotificationPreferencesAsync(Guid studentId, NotificationPreferences notificationPreferences)
        {
            var userNotificationsDocument = await _repository.GetAsync(x => x.StudentId == studentId);

            if (userNotificationsDocument == null)
            {
                userNotificationsDocument = new UserNotificationsDocument
                {
                    Id = Guid.NewGuid(),
                    StudentId = studentId,
                    NotificationPreferences = notificationPreferences
                };

                await _repository.AddAsync(userNotificationsDocument);
            }
            else
            {
                userNotificationsDocument.NotificationPreferences = notificationPreferences;
                await _repository.UpdateAsync(userNotificationsDocument);
            }
        }
    }
}
