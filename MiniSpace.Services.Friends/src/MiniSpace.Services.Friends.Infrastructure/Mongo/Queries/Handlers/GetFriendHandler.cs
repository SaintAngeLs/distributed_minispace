using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Queries;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Queries.Handlers
{
    public class GetFriendHandler : IQueryHandler<GetFriend, FriendDto>
    {
        private readonly IMongoRepository<FriendDocument, Guid> _friendRepository;

        public GetFriendHandler(IMongoRepository<FriendDocument, Guid> friendRepository)
        {
            _friendRepository = friendRepository;
        }

        public async Task<FriendDto> HandleAsync(GetFriend query, CancellationToken cancellationToken)
        {
            var document = await _friendRepository.GetAsync(p => p.Id == query.UserId);
            if (document == null)
                throw new FriendshipNotFoundException(query.UserId);

            return document.AsDto();
        }
    }
}
