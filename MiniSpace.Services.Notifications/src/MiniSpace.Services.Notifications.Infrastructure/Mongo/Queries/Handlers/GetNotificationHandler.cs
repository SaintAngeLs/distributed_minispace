using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Exceptions;
using MiniSpace.Services.Notifications.Application.Queries;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Notifications.Infrastructure.Mongo.Queries.Handlers
{
    public class GetNotificationHandler : IQueryHandler<GetNotification, NotificationDto>
    {
        private readonly IMongoRepository<UserNotificationsDocument, Guid> _repository;

        public GetNotificationHandler(IMongoRepository<UserNotificationsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<NotificationDto> HandleAsync(GetNotification query, CancellationToken cancellationToken)
        {
            var document = await _repository.GetAsync(d => d.UserId == query.UserId);

            if (document == null)
            {
                throw new NotificationNotFoundException(query.UserId, $"User with ID {query.UserId} not found.");
            }

            var notification = document.Notifications.FirstOrDefault(n => n.NotificationId == query.NotificationId);
            if (notification == null)
            {
                throw new NotificationNotFoundException(query.NotificationId);
            }

            return notification.AsDto();
        }
    }
}
