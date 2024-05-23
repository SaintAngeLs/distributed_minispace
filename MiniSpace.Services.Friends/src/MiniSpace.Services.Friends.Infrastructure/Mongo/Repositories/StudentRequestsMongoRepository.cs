using Convey.Persistence.MongoDB;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Repositories
{
    public class StudentRequestsMongoRepository : IStudentRequestsRepository
    {
        private readonly IMongoRepository<StudentRequestsDocument, Guid> _repository;

        public StudentRequestsMongoRepository(IMongoRepository<StudentRequestsDocument, Guid> repository)
        {
            _repository = repository;
        }
     
        public async Task<StudentRequests> GetAsync(Guid studentId)
        {
            // Console.WriteLine($"{studentId}");
            var document = await _repository.FindAsync(doc => doc.StudentId == studentId);
            var studentRequestDocument = document.SingleOrDefault();
            if (studentRequestDocument == null)
            {
                return null;
            }

            var entity = studentRequestDocument.AsEntity();
            var json = JsonSerializer.Serialize(entity, new JsonSerializerOptions { WriteIndented = true });
            // Console.WriteLine(json);

            return entity;
        }


        public async Task<IEnumerable<StudentRequests>> GetAllAsync()
        {
            var documents = await _repository.FindAsync(_ => true);
            return documents.Select(doc => doc.AsEntity()); 
        }

        public async Task AddAsync(StudentRequests studentRequests)
        {
            var document = studentRequests.AsDocument(); 
            await _repository.AddAsync(document);
        }

        public async Task UpdateAsync(StudentRequests studentRequests)
        {
            var document = studentRequests.AsDocument();
            await _repository.UpdateAsync(document);
        }

        public async Task UpdateAsync(Guid studentId, IEnumerable<FriendRequest> updatedFriendRequests)
        {
            var document = await _repository.FindAsync(doc => doc.StudentId == studentId);
            var studentRequestDocument = document.SingleOrDefault();
            // Console.WriteLine($"*******************************************************************************");
            if (studentRequestDocument == null)
            {
                // Console.WriteLine($"No document found with Student ID: {studentId}");
                return; // Consider handling this case appropriately, possibly by adding a new document.
            }

            // Console.WriteLine($"Before update - Document JSON: {JsonSerializer.Serialize(studentRequestDocument, new JsonSerializerOptions { WriteIndented = true })}");

            // Convert each FriendRequest to a FriendRequestDocument before assignment
            studentRequestDocument.FriendRequests = updatedFriendRequests.Select(fr => fr.AsDocument()).ToList();

            var filter = Builders<StudentRequestsDocument>.Filter.Eq(doc => doc.StudentId, studentRequestDocument.StudentId);
            var update = Builders<StudentRequestsDocument>.Update.Set(doc => doc.FriendRequests, studentRequestDocument.FriendRequests);

            var result = await _repository.Collection.UpdateOneAsync(filter, update);

            // Fetch the updated document to log its new state
            var updatedDocument = await _repository.FindAsync(doc => doc.StudentId == studentId);
            var updatedStudentRequestDocument = updatedDocument.SingleOrDefault();

            // Console.WriteLine($"After update - Document JSON: {JsonSerializer.Serialize(updatedStudentRequestDocument, new JsonSerializerOptions { WriteIndented = true })}");

            if (result.ModifiedCount == 0)
            {
                // Console.WriteLine("No documents were modified during the update operation.");
                throw new Exception("Update failed, no document was modified.");
            }
            else
            {
                // Console.WriteLine($"Document with Student ID: {studentId} was successfully updated. Modified count: {result.ModifiedCount}");
            }
        }

        public async Task DeleteAsync(Guid studentId)
        {
            var documents = await _repository.FindAsync(doc => doc.StudentId == studentId);
            var document = documents.SingleOrDefault();
            if (document != null)
            {
                await _repository.DeleteAsync(document.Id);
            }
        }
    }
}
