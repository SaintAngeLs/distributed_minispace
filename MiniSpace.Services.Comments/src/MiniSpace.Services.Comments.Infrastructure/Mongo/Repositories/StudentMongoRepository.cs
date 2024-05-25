using Convey.Persistence.MongoDB;
using MiniSpace.Services.Comments.Core.Entities;
using MiniSpace.Services.Comments.Core.Repositories;
using MiniSpace.Services.Comments.Infrastructure.Mongo.Documents;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Repositories
{
    [ExcludeFromCodeCoverage]
    public class StudentMongoRepository : IStudentRepository
    {
        private readonly IMongoRepository<StudentDocument, Guid> _repository;

        public StudentMongoRepository(IMongoRepository<StudentDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<Student> GetAsync(Guid id)
        {
            var student = await _repository.GetAsync(s => s.Id == id);

            return student?.AsEntity();
        }

        public Task<bool> ExistsAsync(Guid id)
            => _repository.ExistsAsync(s => s.Id == id);

        public Task AddAsync(Student student)
            => _repository.AddAsync(student.AsDocument());

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
    }    
}
