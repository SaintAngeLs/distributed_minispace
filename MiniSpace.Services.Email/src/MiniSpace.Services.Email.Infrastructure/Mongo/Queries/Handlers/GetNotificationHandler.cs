using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Email.Application.Dto;
using MiniSpace.Services.Email.Application.Exceptions;
using MiniSpace.Services.Email.Application.Queries;
using MiniSpace.Services.Email.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Email.Infrastructure.Mongo.Queries.Handlers
{
    public class GetNotificationHandler : IQueryHandler<GetNotification, NotificationDto>
    {
        private readonly IMongoRepository<StudentNotificationsDocument, Guid> _repository;

        public GetNotificationHandler(IMongoRepository<StudentNotificationsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<NotificationDto> HandleAsync(GetNotification query, CancellationToken cancellationToken)
        {
            var document = await _repository.GetAsync(d => d.StudentId == query.UserId);

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
