using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Application.Queries;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;
using System;
using System.Threading.Tasks;
using System.Threading;
using MiniSpace.Services.Friends.Application.Exceptions;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Queries.Handlers
{
    public class PendingFriendAcceptQueryHandler : IQueryHandler<PendingFriendAcceptQuery, FriendRequestDto>
    {
        private readonly IMongoRepository<FriendRequestDocument, Guid> _friendRequestRepository;

        public PendingFriendAcceptQueryHandler(IMongoRepository<FriendRequestDocument, Guid> friendRequestRepository)
        {
            _friendRequestRepository = friendRequestRepository;
        }

        public async Task<FriendRequestDto> HandleAsync(PendingFriendAcceptQuery query, CancellationToken cancellationToken)
        {
            var document = await _friendRequestRepository.GetAsync(p => p.Id == query.FriendRequestId);
            if (document == null)
            {
                 throw new FriendshipNotFoundException(query.FriendRequestId);
            }


            document.State = Core.Entities.FriendState.Accepted; 
            await _friendRequestRepository.UpdateAsync(document);

            return document.AsDto();
        }
    }
}
