using System.Diagnostics.CodeAnalysis;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Core.Repositories;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Repositories
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


        public Task AddAsync(Student student) => _repository.AddAsync(student.AsDocument());


        // public Task AddAsync(Student student)
        // {
        //     throw new NotImplementedException();
        // }

        // public Task DeleteAsync(Guid id)
        // {
        //     throw new NotImplementedException();
        // }
    }    
}
