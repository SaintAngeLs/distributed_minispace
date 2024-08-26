using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Events.Application;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Queries;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Application.Services.Clients;
using MiniSpace.Services.Events.Core.Entities;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    public class GetEventHandler : IQueryHandler<GetEvent, EventDto>
    {
        private readonly IMongoRepository<EventDocument, Guid> _eventRepository;
        private readonly IFriendsServiceClient _friendsServiceClient;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public GetEventHandler(IMongoRepository<EventDocument, Guid> eventRepository, 
            IFriendsServiceClient friendsServiceClient, IAppContext appContext, IMessageBroker messageBroker)
        {
            _eventRepository = eventRepository;
            _friendsServiceClient = friendsServiceClient;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }
        
        public async Task<EventDto> HandleAsync(GetEvent query, CancellationToken cancellationToken)
        {
            var document = await _eventRepository.GetAsync(p => p.Id == query.EventId);
            if (document == null)
            {
                return null;
            }
            
            var identity = _appContext.Identity;
            var friends = Enumerable.Empty<FriendDto>();

            if (identity.IsAuthenticated)
            {
                try
                {
                    var userFriends = await _friendsServiceClient.GetAsync(identity.Id);
                    if (userFriends != null && userFriends.Any())
                    {
                        friends = userFriends.SelectMany(uf => uf.Friends);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error fetching friends: {ex.Message}");
                    throw new ApplicationException("An error occurred while fetching friends data.", ex);
                }
            }

            await _messageBroker.PublishAsync(new EventViewed(query.EventId));
            return document.AsDtoWithFriends(identity.Id, friends);
        }
    }
}
