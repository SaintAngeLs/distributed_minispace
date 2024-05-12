using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Queries;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using MiniSpace.Services.Notifications.Application.Exceptions;

namespace MiniSpace.Services.Notifications.Infrastructure.Mongo.Queries.Handlers
{
    public class GetNotificationHandler : IQueryHandler<GetNotification, NotificationDto>
    {
        private readonly IMongoRepository<NotificationDocument, Guid> _repository;

        public GetNotificationHandler(IMongoRepository<NotificationDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<NotificationDto> HandleAsync(GetNotification query, CancellationToken cancellationToken)
        {
            var document = await _repository.GetAsync(p => p.NotificationId == query.NotificationId);
            if (document == null)
            {
                throw new NotificationNotFoundException(query.NotificationId);
            }

            return document.AsDto();
        }
    }
}
