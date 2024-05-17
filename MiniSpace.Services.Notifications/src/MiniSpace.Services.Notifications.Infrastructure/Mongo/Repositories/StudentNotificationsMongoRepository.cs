using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents;
using MongoDB.Driver;

namespace MiniSpace.Services.Notifications.Infrastructure.Mongo.Repositories
{
    public class StudentNotificationsMongoRepository : IStudentNotificationsRepository
    {
        private readonly IMongoRepository<StudentNotificationsDocument, Guid> _repository;

        public StudentNotificationsMongoRepository(IMongoRepository<StudentNotificationsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<StudentNotifications> GetByStudentIdAsync(Guid studentId)
        {
            var document = await _repository.GetAsync(d => d.StudentId == studentId);
            return document?.AsEntity();
        }

        public async Task AddAsync(StudentNotifications studentNotifications)
        {
            var document = studentNotifications.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task UpdateAsync(StudentNotifications studentNotifications)
        {
            var filter = Builders<StudentNotificationsDocument>.Filter.Eq(doc => doc.StudentId, studentNotifications.StudentId);
            var update = Builders<StudentNotificationsDocument>.Update
                .SetOnInsert(doc => doc.Id, studentNotifications.StudentId) 
                .PushEach(doc => doc.Notifications, studentNotifications.Notifications.Select(n => n.AsDocument()));

            var options = new UpdateOptions { IsUpsert = true };
            await _repository.Collection.UpdateOneAsync(filter, update, options);
        }

        public Task DeleteAsync(Guid studentId)
        {
            return _repository.DeleteAsync(studentId);
        }
    }
}
