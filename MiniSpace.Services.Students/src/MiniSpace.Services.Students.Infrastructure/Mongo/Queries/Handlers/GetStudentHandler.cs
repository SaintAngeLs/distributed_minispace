using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Students.Application;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Queries.Handlers
{
    public class GetStudentHandler : IQueryHandler<GetStudent, StudentDto>
    {
        private readonly IMongoRepository<StudentDocument, Guid> _studentRepository;
        private readonly IAppContext _appContext;

        public GetStudentHandler(IMongoRepository<StudentDocument, Guid> studentRepository, IAppContext appContext)
        {
            _studentRepository = studentRepository;
            _appContext = appContext;
        }
        
        public async Task<StudentDto> HandleAsync(GetStudent query, CancellationToken cancellationToken)
        {
            var document = await _studentRepository.GetAsync(p => p.Id == query.StudentId);

            var identity = _appContext.Identity;
            return document?.AsDto(identity.IsBanned, identity.IsOrganizer);
        }
    }    
}
