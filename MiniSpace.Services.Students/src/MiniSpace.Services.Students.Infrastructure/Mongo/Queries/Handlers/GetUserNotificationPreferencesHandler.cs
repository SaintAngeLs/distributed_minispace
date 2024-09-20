using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Students.Infrastructure.Mongo.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Queries.Handlers
{
    public class GetUserNotificationPreferencesHandler : IQueryHandler<GetUserNotificationPreferences, NotificationPreferencesDto>
    {
        private readonly IMongoRepository<UserNotificationsDocument, Guid> _repository;

        public GetUserNotificationPreferencesHandler(IMongoRepository<UserNotificationsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<NotificationPreferencesDto> HandleAsync(GetUserNotificationPreferences query, CancellationToken cancellationToken)
        {
            var userNotificationsDocument = await _repository.GetAsync(x => x.UserId == query.StudentId);
            return userNotificationsDocument?.NotificationPreferences.AsDto();
        }
    }
}
