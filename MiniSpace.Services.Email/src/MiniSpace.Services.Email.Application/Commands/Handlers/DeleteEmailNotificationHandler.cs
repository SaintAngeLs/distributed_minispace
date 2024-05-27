// using Convey.CQRS.Commands;
// using MiniSpace.Services.Email.Core.Repositories;
// using MiniSpace.Services.Email.Application.Exceptions;
// using MiniSpace.Services.Email.Application.Services;
// using System.Threading.Tasks;
// using System;
// using MiniSpace.Services.Email.Application.Events.External;

// namespace MiniSpace.Services.Email.Application.Commands.Handlers
// {
//     public class DeleteNotificationHandler : ICommandHandler<DeleteNotification>
//     {
//         private readonly IStudentNotificationsRepository _studentNotificationsRepository;
//         private readonly IEventMapper _eventMapper;
//         private readonly IMessageBroker _messageBroker;

//         public DeleteNotificationHandler(IStudentNotificationsRepository notificationRepository, IEventMapper eventMapper, IMessageBroker messageBroker)
//         {
//             _studentNotificationsRepository = notificationRepository;
//             _eventMapper = eventMapper;
//             _messageBroker = messageBroker;
//         }

//         public async Task HandleAsync(DeleteNotification command, CancellationToken cancellationToken = default)
//         {
//             var exists = await _studentNotificationsRepository.NotificationExists(command.UserId, command.NotificationId);
//             if (!exists)
//             {
//                 throw new NotificationNotFoundException(command.NotificationId);
//             }

//             await _studentNotificationsRepository.DeleteNotification(command.UserId, command.NotificationId);
          
//             var notificationDeletedEvent = new NotificationDeleted(command.UserId, command.NotificationId);
//             await _messageBroker.PublishAsync(notificationDeletedEvent);
//         }
//     }
// }
