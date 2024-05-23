using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Queries;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Notifications.Infrastructure.Mongo.Queries.Handlers
{
    public class GetNotificationsByUserHandler : IQueryHandler<GetNotificationsByUser, Application.Queries.PagedResult<NotificationDto>>
    {
        private readonly IMongoRepository<StudentNotificationsDocument, Guid> _repository;
        private const string BaseUrl = "notifications"; 

        public GetNotificationsByUserHandler(IMongoRepository<StudentNotificationsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Application.Queries.PagedResult<NotificationDto>> HandleAsync(GetNotificationsByUser query, CancellationToken cancellationToken)
        {
            var filter = Builders<StudentNotificationsDocument>.Filter.Eq(doc => doc.StudentId, query.UserId);
            var sortBy = Builders<StudentNotificationsDocument>.Sort.Descending(query.OrderBy); 

            if (query.SortOrder.ToLower() == "asc")
            {
                sortBy = Builders<StudentNotificationsDocument>.Sort.Ascending(query.OrderBy);
            }

            var options = new FindOptions<StudentNotificationsDocument, StudentNotificationsDocument>
            {
                Limit = query.ResultsPerPage,
                Skip = (query.Page - 1) * query.ResultsPerPage,
                Sort = sortBy
            };

            using (var cursor = await _repository.Collection.FindAsync(filter, options, cancellationToken))
            {
                var documents = await cursor.ToListAsync(cancellationToken);
                var notificationDtos = documents
                    .SelectMany(doc => doc.Notifications
                        .Where(n => (!query.StartDate.HasValue || n.CreatedAt >= query.StartDate.Value) &&
                                    (!query.EndDate.HasValue || n.CreatedAt <= query.EndDate.Value))
                        .Select(n => n.AsDto()))
                    .ToList();

                var total = await _repository.Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

                return new Application.Queries.PagedResult<NotificationDto>(notificationDtos, (int)total, query.ResultsPerPage, query.Page, BaseUrl);
            }
        }
    }
}
