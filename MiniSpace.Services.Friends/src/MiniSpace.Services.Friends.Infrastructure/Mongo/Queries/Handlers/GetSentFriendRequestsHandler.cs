using Paralax.CQRS.Queries;
using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Application.Queries;
using MiniSpace.Services.Friends.Core.Wrappers;
using MiniSpace.Services.Friends.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Friends.Infrastructure.Mongo.Queries.Handlers
{
    public class GetSentFriendRequestsHandler : IQueryHandler<GetSentFriendRequests, PagedResponse<UserRequestsDto>>
    {
        private readonly IMongoRepository<UserRequestsDocument, Guid> _userRequestsRepository;

        public GetSentFriendRequestsHandler(IMongoRepository<UserRequestsDocument, Guid> userRequestsRepository)
        {
            _userRequestsRepository = userRequestsRepository;
        }

        public async Task<PagedResponse<UserRequestsDto>> HandleAsync(GetSentFriendRequests query, CancellationToken cancellationToken)
        {
            // Fetch user requests from the database
            var userRequests = await _userRequestsRepository.Collection
                .Find(doc => doc.UserId == query.UserId)
                .ToListAsync(cancellationToken);

            if (userRequests == null || !userRequests.Any())
            {
                return new PagedResponse<UserRequestsDto>(Enumerable.Empty<UserRequestsDto>(), query.Page, query.PageSize, 0);
            }

            // Filter sent friend requests and map them to DTOs
            var sentRequests = userRequests
                .Select(doc => new UserRequestsDto
                {
                    Id = doc.Id,
                    UserId = doc.UserId,
                    FriendRequests = doc.FriendRequests
                        .Where(request => request.InviterId == query.UserId && request.State == Core.Entities.FriendState.Requested)
                        .Select(request => new FriendRequestDto
                        {
                            Id = request.Id,
                            InviterId = request.InviterId,
                            InviteeId = request.InviteeId,
                            RequestedAt = request.RequestedAt,
                            State = request.State
                        })
                        .ToList()
                })
                .Where(dto => dto.FriendRequests.Any())
                .ToList();

            // Implement pagination
            var totalItems = sentRequests.Count;
            var paginatedRequests = sentRequests
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            // Return the paginated response
            return new PagedResponse<UserRequestsDto>(paginatedRequests, query.Page, query.PageSize, totalItems);
        }
    }
}
