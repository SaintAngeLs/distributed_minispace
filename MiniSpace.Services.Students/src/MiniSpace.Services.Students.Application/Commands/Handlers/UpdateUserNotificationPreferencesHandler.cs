using Paralax.CQRS.Commands;
using MiniSpace.Services.Students.Core.Entities;
using MiniSpace.Services.Students.Core.Repositories;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Commands.Handlers
{
    public class UpdateUserNotificationPreferencesHandler : ICommandHandler<UpdateUserNotificationPreferences>
    {
        private readonly IUserNotificationPreferencesRepository _userNotificationPreferencesRepository;
        private readonly IStudentRepository _studentRepository;

        public UpdateUserNotificationPreferencesHandler(IUserNotificationPreferencesRepository userNotificationPreferencesRepository, IStudentRepository studentRepository)
        {
            _userNotificationPreferencesRepository = userNotificationPreferencesRepository;
            _studentRepository = studentRepository;
        }

        public async Task HandleAsync(UpdateUserNotificationPreferences command, CancellationToken cancellationToken = default)
        {
            var commandJson = JsonSerializer.Serialize(command);
            Console.WriteLine($"Received UpdateUserNotificationPreferences command: {commandJson}");

            var notificationPreferences = new NotificationPreferences(
                command.SystemLogin,
                command.InterestBasedEvents,
                command.EventNotifications,
                command.CommentsNotifications,
                command.PostsNotifications,
                command.EventRecommendation,
                command.FriendsRecommendation,
                command.FriendsPosts,
                command.PostsRecommendation,
                command.EventsIAmInterestedInNotification,
                command.EventsIAmSignedUpToNotification,
                command.PostsOfPeopleIFollowNotification,
                command.EventNotificationForPeopleIFollow,
                command.NewFriendsRequests, 
                command.MyRequestsAccepted, 
                command.FriendsPostsNotifications 
            );

            await _userNotificationPreferencesRepository.UpdateNotificationPreferencesAsync(command.UserId, notificationPreferences);

            var student = await _studentRepository.GetAsync(command.UserId);
            if (student != null)
            {
                student.SetEmailNotifications(command.EmailNotifications);
                await _studentRepository.UpdateAsync(student);
            }
        }
    }
}
