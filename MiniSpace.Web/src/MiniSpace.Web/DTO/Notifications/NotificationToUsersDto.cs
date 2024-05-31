using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Web.DTO.Notifications
{
    public class NotificationToUsersDto
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public IEnumerable<Guid> StudentIds { get; set; }
        public Guid EventId { get; set; }
        public NotificationToUsersDto() { }

        public NotificationToUsersDto(Guid notificationId, Guid userId, string message, IEnumerable<Guid> studentIds, Guid eventId)
        {
            NotificationId = notificationId;
            UserId = userId;
            Message = message;
            StudentIds = studentIds;
            EventId = eventId;
        }
    }
}
