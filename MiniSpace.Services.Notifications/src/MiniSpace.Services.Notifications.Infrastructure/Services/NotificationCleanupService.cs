using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Repositories;
using MongoDB.Driver;
using MiniSpace.Services.Notifications.Application.Services.Clients;

public class NotificationCleanupService : BackgroundService
{
    private readonly ILogger<NotificationCleanupService> _logger;
    private readonly IExtendedStudentNotificationsRepository _notificationsRepository;
    private readonly IStudentsServiceClient _studentsServiceClient;

    public NotificationCleanupService(ILogger<NotificationCleanupService> logger, 
                                      IExtendedStudentNotificationsRepository notificationsRepository,
                                      IStudentsServiceClient studentsServiceClient) 
    {
        _logger = logger;
        _notificationsRepository = notificationsRepository;
        _studentsServiceClient = studentsServiceClient; 
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var dailyCleanupTask = Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Daily cleanup of read notifications is running.");
                await RemoveReadNotifications();
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        });

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("20-minute cleanup service is running.");
            await CleanupOldNotifications();
            _logger.LogInformation("Waiting 20 minutes before next cleanup.");
            await Task.Delay(TimeSpan.FromMinutes(20), stoppingToken); 
        }

        await dailyCleanupTask;
    }

    private async Task CleanupOldNotifications()
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-28);
        var allStudents = await _studentsServiceClient.GetAllAsync();

        if (allStudents != null)
        {
            foreach (var student in allStudents)
            {
                var studentId = student.Id;
                var count = await _notificationsRepository.GetNotificationCount(studentId);
                if (count > 300)
                {
                    _logger.LogInformation($"Cleaning up old notifications for student {studentId}.");
                    var filter = Builders<StudentNotificationsDocument>.Filter.And(
                        Builders<StudentNotificationsDocument>.Filter.Eq(doc => doc.StudentId, studentId),
                        Builders<StudentNotificationsDocument>.Filter.ElemMatch(n => n.Notifications, n => n.CreatedAt < cutoffDate)
                    );

                    var update = Builders<StudentNotificationsDocument>.Update.PullFilter(
                        n => n.Notifications, n => n.CreatedAt < cutoffDate);

                    var result = await _notificationsRepository.BulkUpdateAsync(filter, update);
                    _logger.LogInformation($"Removed {result.ModifiedCount} old notifications for student {studentId}.");
                }
            }
        }
        else
        {
            _logger.LogError("Failed to fetch student data from the students service.");
        }
    }

    private async Task RemoveReadNotifications()
    {
        var allStudents = await _studentsServiceClient.GetAllAsync();
        if (allStudents != null)
        {
            foreach (var student in allStudents)
            {
                var studentId = student.Id;
                var filter = Builders<StudentNotificationsDocument>.Filter.And(
                    Builders<StudentNotificationsDocument>.Filter.Eq(doc => doc.StudentId, studentId),
                    Builders<StudentNotificationsDocument>.Filter.ElemMatch(n => n.Notifications, n => n.Status == "Read")
                );

                var update = Builders<StudentNotificationsDocument>.Update.PullFilter(
                    n => n.Notifications, n => n.Status == "Read");

                var result = await _notificationsRepository.BulkUpdateAsync(filter, update);
                _logger.LogInformation($"Removed all read notifications for student {studentId}.");
            }
        }
        else
        {
            _logger.LogError("Failed to fetch student data from the students service.");
        }
    }
}
