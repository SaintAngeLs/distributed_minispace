using Convey.Persistence.MongoDB;
using MiniSpace.Services.Notifications.Core.Entities;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniSpace.Services.Notifications.Infrastructure.Mongo.Repositories
{
    public class StudentMongoRepository : IStudentRepository
    {
        private readonly IMongoRepository<StudentDocument, Guid> _repository;

        public StudentMongoRepository(IMongoRepository<StudentDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Student> GetAsync(Guid id)
        {
            var document = await _repository.GetAsync(o => o.Id == id);
            return document?.AsEntity();
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            var documents = await _repository.FindAsync(_ => true);
            List<Student> students = new List<Student>();
            foreach (var doc in documents)
            {
                students.Add(doc.AsEntity());
            }
            return students;
        }

        public Task AddAsync(Student student)
        {
            return _repository.AddAsync(student.AsDocument());
        }

        public async Task UpdateAsync(Student student)
        {
            var filter = Builders<StudentDocument>.Filter.Eq(doc => doc.Id, student.Id);
            var update = Builders<StudentDocument>.Update
                .Set(doc => doc.Name, student.Name)
                .Set(doc => doc.ProfileImage, student.ProfileImage);

            await _repository.Collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(Guid id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
