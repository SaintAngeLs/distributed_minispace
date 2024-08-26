using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents;
using MongoDB.Driver;
using MiniSpace.Services.Communication.Application.Services.Clients;
using MiniSpace.Services.Communication.Infrastructure.Mongo.Repositories;

public class DailyNotificationCleanupService : BackgroundService
{
    private readonly ILogger<DailyNotificationCleanupService> _logger;
    private readonly IExtendedStudentNotificationsRepository _notificationsRepository;
    private readonly IStudentsServiceClient _studentsServiceClient;

    public DailyNotificationCleanupService(ILogger<DailyNotificationCleanupService> logger, 
                                           IExtendedStudentNotificationsRepository notificationsRepository,
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
            _logger.LogInformation("Daily cleanup of read notifications is running.");
            await RemoveReadNotifications(stoppingToken);
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    private async Task RemoveReadNotifications(CancellationToken stoppingToken)
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
