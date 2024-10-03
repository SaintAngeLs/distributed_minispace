using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Repositories;

public class PeriodicNotificationCleanupService : BackgroundService
{
    private readonly ILogger<PeriodicNotificationCleanupService> _logger;
    private readonly IExtendedUserNotificationsRepository _notificationsRepository;
    private readonly IStudentsServiceClient _studentsServiceClient;

    public PeriodicNotificationCleanupService(ILogger<PeriodicNotificationCleanupService> logger, 
                                              IExtendedUserNotificationsRepository notificationsRepository,
                                              IStudentsServiceClient studentsServiceClient) 
    {
        _logger = logger;
        _notificationsRepository = notificationsRepository;
        _studentsServiceClient = studentsServiceClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("20-minute cleanup service is running.");
            await CleanupOldNotifications(stoppingToken);
            _logger.LogInformation("Waiting 10 minutes before next cleanup.");
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); 
        }
    }

    private async Task CleanupOldNotifications(CancellationToken stoppingToken)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-1);
        var allStudents = await _studentsServiceClient.GetAllAsync();

        if (allStudents != null)
        {
            foreach (var student in allStudents)
            {
                var studentId = student.Id;
                var count = await _notificationsRepository.GetNotificationCount(studentId);
                _logger.LogInformation($"Searching {studentId} : {count}.");
                    

                if (count >= 300)
                {
                    _logger.LogInformation($"Removing all notifications for student {studentId} due to excessive count: {count}.");
                    
                    
                    var filter = Builders<UserNotificationsDocument>.Filter.Eq(doc => doc.UserId, studentId);
                    var update = Builders<UserNotificationsDocument>.Update.Set(doc => doc.Notifications, new List<NotificationDocument>());
                    var result = await _notificationsRepository.BulkUpdateAsync(filter, update);

                    _logger.LogInformation($"All notifications for student {studentId} have been removed. Total removed: {result.ModifiedCount}");
                }
                else if (count > 200)
                {
                    _logger.LogInformation($"Cleaning up old notifications for student {studentId}. Total current count: {count}.");

                    var filter = Builders<UserNotificationsDocument>.Filter.And(
                        Builders<UserNotificationsDocument>.Filter.Eq(doc => doc.UserId, studentId),
                        Builders<UserNotificationsDocument>.Filter.Lt("Notifications.CreatedAt", cutoffDate)
                    );

                    var update = Builders<UserNotificationsDocument>.Update.PullFilter(
                        n => n.Notifications, n => n.CreatedAt < cutoffDate);

                    var result = await _notificationsRepository.BulkUpdateAsync(filter, update);

                    _logger.LogInformation($"Removed old notifications for student {studentId}. Total old notifications removed: {result.ModifiedCount}.");
                }
            }
        }
        else
        {
            _logger.LogError("Failed to fetch student data from the students service.");
        }
    }
}
