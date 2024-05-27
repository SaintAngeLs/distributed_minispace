using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiniSpace.Services.Email.Core.Repositories;
using MiniSpace.Services.Email.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Email.Infrastructure.Mongo.Repositories;
using MongoDB.Driver;

public class NotificationCleanupService : BackgroundService
{
    private readonly ILogger<NotificationCleanupService> _logger;
    private readonly IExtendedStudentNotificationsRepository _notificationsRepository;

    public NotificationCleanupService(ILogger<NotificationCleanupService> logger, IExtendedStudentNotificationsRepository notificationsRepository)
    {
        _logger = logger;
        _notificationsRepository = notificationsRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Notification Cleanup Service is running.");
            await CleanupOldNotifications();
            _logger.LogInformation("Waiting 4 weeks before next cleanup.");
            await Task.Delay(TimeSpan.FromDays(28), stoppingToken);
        }
    }

    private async Task CleanupOldNotifications()
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-28);
        var filter = Builders<StudentNotificationsDocument>.Filter.ElemMatch(n => n.Notifications, n => n.CreatedAt < cutoffDate);
        var update = Builders<StudentNotificationsDocument>.Update.PullFilter(n => n.Notifications, n => n.CreatedAt < cutoffDate);
        var result = await _notificationsRepository.BulkUpdateAsync(filter, update);
        
        _logger.LogInformation($"Removed {result.ModifiedCount} old notifications.");
    }
}
