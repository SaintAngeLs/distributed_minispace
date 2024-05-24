using Convey.CQRS.Queries;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Core.Entities;
using System;

namespace MiniSpace.Services.Notifications.Application.Queries
{
    public class GetNotificationsByUser : IQuery<PagedResult<NotificationDto>>, IPagedNotificationsQuery
    {
        public Guid UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public int Page { get; set; } = 1;
        public int ResultsPerPage { get; set; } = 10;
        public string OrderBy { get; set; } = "CreatedAt"; 
        public string SortOrder { get; set; } = "desc";    
    }
}
