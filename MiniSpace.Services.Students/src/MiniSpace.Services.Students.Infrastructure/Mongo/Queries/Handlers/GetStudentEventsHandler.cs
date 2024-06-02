using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetStudentEventsHandler : IQueryHandler<GetStudentEvents, StudentEventsDto>
    {
        private readonly IMongoRepository<StudentDocument, Guid> _studentRepository;

        public GetStudentEventsHandler(IMongoRepository<StudentDocument, Guid> repository)
            => _studentRepository = repository;

        public async Task<StudentEventsDto> HandleAsync(GetStudentEvents query, CancellationToken cancellationToken)
        {
            var document = await _studentRepository.GetAsync(p => p.Id == query.StudentId);
            if(document is null)
            {
                throw new StudentNotFoundException(query.StudentId);
            }
            
            var studentEvents = new StudentEventsDto()
            {
                StudentId = document.Id,
                InterestedInEvents = document.InterestedInEvents,
                SignedUpEvents = document.SignedUpEvents
            };

            return studentEvents;
        }
    }
}