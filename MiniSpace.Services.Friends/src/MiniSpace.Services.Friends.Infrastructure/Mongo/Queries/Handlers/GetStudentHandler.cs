using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Application.Queries;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Queries.Handlers
{
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
