using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Queries;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

            if (!string.IsNullOrEmpty(query.Status))
            {
                var regex = new MongoDB.Bson.BsonRegularExpression($"^{Regex.Escape(query.Status)}$", "i");
                var statusFilter = Builders<StudentNotificationsDocument>.Filter.ElemMatch(x => x.Notifications,
                    Builders<NotificationDocument>.Filter.Regex("Status", regex));

                filter = Builders<StudentNotificationsDocument>.Filter.And(filter, statusFilter);

                Console.WriteLine($"Applying status filter with regex: {regex}");
            }

            var sortBy = Builders<StudentNotificationsDocument>.Sort.Descending(doc => doc.Notifications[-1].CreatedAt);

            if (query.SortOrder.ToLower() == "asc")
            {
                sortBy = Builders<StudentNotificationsDocument>.Sort.Ascending(doc => doc.Notifications[-1].CreatedAt);
            }

            var notificationsList = new List<NotificationDto>();

            var documents = await _repository.Collection.Find(filter).ToListAsync(cancellationToken);
            
            var allNotifications = documents.SelectMany(doc => doc.Notifications
                .Where(n => (!query.StartDate.HasValue || n.CreatedAt >= query.StartDate.Value) &&
                            (!query.EndDate.HasValue || n.CreatedAt <= query.EndDate.Value) &&
                            (string.IsNullOrEmpty(query.Status) || n.Status.Equals(query.Status, StringComparison.OrdinalIgnoreCase)))
                .Select(n => n.AsDto()))
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

            if (query.SortOrder.ToLower() == "asc")
            {
                allNotifications = allNotifications.OrderBy(n => n.CreatedAt).ToList();
            }

            var totalNotifications = allNotifications.Count;
            var paginatedNotifications = allNotifications
                .Skip((query.Page - 1) * query.ResultsPerPage)
                .Take(query.ResultsPerPage)
                .ToList();

            return new Application.Queries.PagedResult<NotificationDto>(paginatedNotifications, totalNotifications, query.ResultsPerPage, query.Page, BaseUrl);
        }
    }
}
