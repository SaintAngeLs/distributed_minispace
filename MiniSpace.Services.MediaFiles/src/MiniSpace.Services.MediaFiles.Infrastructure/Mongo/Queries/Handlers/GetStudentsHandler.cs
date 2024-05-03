using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.MediaFiles.Application.Dto;
using MiniSpace.Services.MediaFiles.Application.Queries;
using MiniSpace.Services.MediaFiles.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Mongo.Queries.Handlers
{
    public class GetStudentsHandler : IQueryHandler<GetStudents, IEnumerable<StudentDto>>
    {
        private readonly IMongoRepository<StudentDocument, Guid> _studentRepository;

        public GetStudentsHandler(IMongoRepository<StudentDocument, Guid> studentRepository)
        {
            _studentRepository = studentRepository;
        }
        
        public async Task<IEnumerable<StudentDto>> HandleAsync(GetStudents query, CancellationToken cancellationToken)
        {
            var students = await _studentRepository.FindAsync(_ => true);

            return students.Select(p => p.AsDto());
        }
    }    
}
