using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Email.Core.Entities;
using MiniSpace.Services.Email.Core.Repositories;
using MiniSpace.Services.Email.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Email.Infrastructure.Mongo.Extensions;
using MongoDB.Driver;

namespace MiniSpace.Services.Email.Infrastructure.Mongo.Repositories
{
    public class StudentEmailsMongoRepository : IStudentEmailsRepository
    {
        private readonly IMongoRepository<StudentEmailsDocument, Guid> _repository;

        public StudentEmailsMongoRepository(IMongoRepository<StudentEmailsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<StudentEmails> GetByStudentIdAsync(Guid studentId)
        {
            var document = await _repository.GetAsync(d => d.StudentId == studentId);
            return document?.AsEntity();
        }

        public async Task AddAsync(StudentEmails studentEmails)
        {
            var document = studentEmails.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task UpdateAsync(StudentEmails studentEmails)
        {
            var filter = Builders<StudentEmailsDocument>.Filter.Eq(doc => doc.StudentId, studentEmails.StudentId);
            var update = Builders<StudentEmailsDocument>.Update
                .SetOnInsert(doc => doc.Id, studentEmails.StudentId)
                .Set(doc => doc.EmailNotifications, studentEmails.EmailNotifications.Select(e => e.AsDocument()).ToList());

            var options = new UpdateOptions { IsUpsert = true };
            await _repository.Collection.UpdateOneAsync(filter, update, options);
        }



        public async Task AddOrUpdateAsync(StudentEmails studentEmails)
        {
            var document = studentEmails.AsDocument();
            var filter = Builders<StudentEmailsDocument>.Filter.Eq(doc => doc.StudentId, studentEmails.StudentId);
            var existingDocument = await _repository.Collection.Find(filter).FirstOrDefaultAsync();

            if (existingDocument == null)
            {
                await _repository.AddAsync(document);
            }
            else
            {
                var update = Builders<StudentEmailsDocument>.Update
                    .Set(doc => doc.EmailNotifications, studentEmails.EmailNotifications.Select(e => e.AsDocument()).ToList());
                await _repository.Collection.ReplaceOneAsync(filter, document);
            }
        }



        public async Task<List<StudentEmails>> FindAsync(FilterDefinition<StudentEmailsDocument> filter, FindOptions options = null)
        {
            var documents = await _repository.Collection.Find(filter, options).ToListAsync();
            return documents.Select(doc => doc.AsEntity()).ToList();
        }


        public async Task DeleteAsync(Guid studentId)
        {
            await _repository.DeleteAsync(studentId);
        }

        public async Task UpdateEmailStatus(Guid studentId, Guid notificationId, EmailNotificationStatus newStatus)
        {
            var filter = Builders<StudentEmailsDocument>.Filter.And(
                Builders<StudentEmailsDocument>.Filter.Eq(doc => doc.StudentId, studentId),
                Builders<StudentEmailsDocument>.Filter.ElemMatch(doc => doc.EmailNotifications, n => n.EmailNotificationId == notificationId)
            );

            var update = Builders<StudentEmailsDocument>.Update
                .Set("EmailNotifications.$.Status", newStatus)
                .CurrentDate("EmailNotifications.$.UpdatedAt");

            await _repository.Collection.UpdateOneAsync(filter, update);
        }
        
        public async Task<bool> EmailExists(Guid studentId, Guid notificationId)
        {
            var filter = Builders<StudentEmailsDocument>.Filter.And(
                Builders<StudentEmailsDocument>.Filter.Eq(doc => doc.StudentId, studentId),
                Builders<StudentEmailsDocument>.Filter.ElemMatch(doc => doc.EmailNotifications, n => n.EmailNotificationId == notificationId)
            );

            var result = await _repository.Collection.Find(filter).AnyAsync();
            return result;
        }

        public async Task DeleteEmail(Guid studentId, Guid notificationId)
        {
            var filter = Builders<StudentEmailsDocument>.Filter.And(
                Builders<StudentEmailsDocument>.Filter.Eq(d => d.StudentId, studentId),
                Builders<StudentEmailsDocument>.Filter.ElemMatch(e => e.EmailNotifications, n => n.EmailNotificationId == notificationId));

            var update = Builders<StudentEmailsDocument>.Update.PullFilter(
                p => p.EmailNotifications, n => n.EmailNotificationId == notificationId);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }
    }
}
