using Convey.Persistence.MongoDB;
using MiniSpace.Services.Friends.Core.Entities;
using MiniSpace.Services.Friends.Core.Repositories;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Repositories
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
            var student = await _repository.GetAsync(o => o.Id == id);

            return student?.AsEntity();
        }

        public Task AddAsync(Student student)
            => _repository.AddAsync(student.AsDocument());

        public Task UpdateAsync(Student student)
            => _repository.UpdateAsync(student.AsDocument());

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
    }    
}
