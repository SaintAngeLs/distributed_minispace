using Paralax.CQRS.Queries;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Exceptions;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetUserEventsHandler : IQueryHandler<GetUserEvents, UserEventsDto>
    {
        private readonly IMongoRepository<UserDocument, Guid> _userRepository;

        public GetUserEventsHandler(IMongoRepository<UserDocument, Guid> repository)
            => _userRepository = repository;

        public async Task<UserEventsDto> HandleAsync(GetUserEvents query, CancellationToken cancellationToken)
        {
            var document = await _userRepository.GetAsync(p => p.Id == query.UserId);
            if(document is null)
            {
                throw new UserNotFoundException(query.UserId);
            }
            
            var studentEvents = new UserEventsDto()
            {
                UserId = document.Id,
                InterestedInEvents = document.InterestedInEvents,
                SignedUpEvents = document.SignedUpEvents
            };

            return studentEvents;
        }
    }
}