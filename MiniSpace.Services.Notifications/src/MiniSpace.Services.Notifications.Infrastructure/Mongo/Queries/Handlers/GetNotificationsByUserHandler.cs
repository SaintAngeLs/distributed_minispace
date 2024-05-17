using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Exceptions;
using MiniSpace.Services.Notifications.Application.Queries;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Notifications.Infrastructure.Mongo.Queries.Handlers
{
    public class GetNotificationsByUserHandler : IQueryHandler<GetNotificationsByUser, IEnumerable<NotificationDto>>
    {
        private readonly IMongoRepository<StudentNotificationsDocument, Guid> _repository;

        public GetNotificationsByUserHandler(IMongoRepository<StudentNotificationsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<NotificationDto>> HandleAsync(GetNotificationsByUser query, CancellationToken cancellationToken)
        {
            var studentNotificationDoc = await _repository.GetAsync(d => d.StudentId == query.UserId);

            return studentNotificationDoc.Notifications.Select(document => document.AsDto());
        }
    }
}
