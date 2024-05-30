using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Email.Application.Dto;
using MiniSpace.Services.Email.Application.Queries;
using MiniSpace.Services.Email.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiniSpace.Services.Email.Infrastructure.Mongo.Queries.Handlers
{
    public class GetEmailNotificationsByUserHandler : IQueryHandler<GetEmailNotificationsByUser, Application.Queries.PagedResult<EmailNotificationDto>>
    {
        private readonly IMongoRepository<StudentEmailsDocument, Guid> _repository;
        private const string BaseUrl = "notifications"; 

        public GetEmailNotificationsByUserHandler(IMongoRepository<StudentEmailsDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<Application.Queries.PagedResult<EmailNotificationDto>> HandleAsync(GetEmailNotificationsByUser query, CancellationToken cancellationToken)
        {
            var filter = Builders<StudentEmailsDocument>.Filter.Eq(doc => doc.StudentId, query.UserId);

            if (!string.IsNullOrEmpty(query.Status))
            {
                var regex = new MongoDB.Bson.BsonRegularExpression($"^{Regex.Escape(query.Status)}$", "i");
                var statusFilter = Builders<StudentEmailsDocument>.Filter.ElemMatch(x => x.EmailNotifications,
                    Builders<EmailNotificationDocument>.Filter.Regex(x => x.Status, regex));

                filter = Builders<StudentEmailsDocument>.Filter.And(filter, statusFilter);
            }

            var sortDefinition = query.SortOrder.ToLower() == "asc" ?
                Builders<StudentEmailsDocument>.Sort.Ascending(doc => doc.EmailNotifications[-1].CreatedAt) :
                Builders<StudentEmailsDocument>.Sort.Descending(doc => doc.EmailNotifications[-1].CreatedAt);

            
            var options = new FindOptions<StudentEmailsDocument, StudentEmailsDocument>
            {
                Sort = sortDefinition,
                Skip = (query.Page - 1) * query.ResultsPerPage,
                Limit = query.ResultsPerPage
            };

            var documents = await _repository.Collection.Find(filter).ToListAsync(cancellationToken);


            var notificationsDtoList = documents.SelectMany(doc => doc.EmailNotifications
                .Where(n => (!query.StartDate.HasValue || n.CreatedAt >= query.StartDate.Value) &&
                            (!query.EndDate.HasValue || n.CreatedAt <= query.EndDate.Value))
                .Select(n => new EmailNotificationDto
                {
                    EmailNotificationId = n.EmailNotificationId,
                    UserId = n.UserId,
                    EmailAddress = n.EmailAddress,
                    Subject = n.Subject,
                    Body = n.Body,
                    Status = n.Status.ToString(),
                    CreatedAt = n.CreatedAt,
                    SentAt = n.SentAt
                }))
                .ToList();

            var totalResults = notificationsDtoList.Count;
            var paginatedResults = notificationsDtoList.Skip((query.Page - 1) * query.ResultsPerPage).Take(query.ResultsPerPage).ToList();

            return new Application.Queries.PagedResult<EmailNotificationDto>(paginatedResults, totalResults, query.ResultsPerPage, query.Page, BaseUrl);
        }
    }
}
