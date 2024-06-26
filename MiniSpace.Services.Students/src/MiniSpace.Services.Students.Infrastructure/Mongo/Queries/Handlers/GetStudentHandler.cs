using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetStudentHandler : IQueryHandler<GetStudent, StudentDto>
    {
        private readonly IMongoRepository<StudentDocument, Guid> _studentRepository;

        public GetStudentHandler(IMongoRepository<StudentDocument, Guid> studentRepository)
        {
            _studentRepository = studentRepository;
        }
        
        public async Task<StudentDto> HandleAsync(GetStudent query, CancellationToken cancellationToken)
        {
            var document = await _studentRepository.GetAsync(p => p.Id == query.StudentId);
            
            return document?.AsDto();
        }
    }    
}
