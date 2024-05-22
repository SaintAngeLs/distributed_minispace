using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.Persistence.MongoDB;
using Microsoft.Extensions.Hosting;
using MiniSpace.Services.MediaFiles.Application.Commands;
using MiniSpace.Services.MediaFiles.Application.Events;
using MiniSpace.Services.MediaFiles.Application.Services;
using MiniSpace.Services.MediaFiles.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Services.Workers
{
    public class FileCleanupWorker: BackgroundService
    {
        private readonly IMessageBroker _messageBroker;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IDateTimeProvider _dateTimeProvider;
        private const int MinutesInterval = 10;
        
        public FileCleanupWorker(IMessageBroker messageBroker, ICommandDispatcher commandDispatcher,
            IDateTimeProvider dateTimeProvider)
        {
            _messageBroker = messageBroker;
            _commandDispatcher = commandDispatcher;
            _dateTimeProvider = dateTimeProvider;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _messageBroker.PublishAsync(new FileCleanupBackgroundWorkerStarted(_dateTimeProvider.Now));
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = _dateTimeProvider.Now;
                    var minutes = now.Minute;
                    if (minutes % MinutesInterval == 0)
                    {
                        await _commandDispatcher.SendAsync(new CleanupUnassociatedFiles(now), stoppingToken);
                    }
                    
                    var nextTime = now.AddMinutes(MinutesInterval - (minutes % MinutesInterval)).AddSeconds(-now.Second)
                        .AddMilliseconds(-now.Millisecond);
                    var delay = nextTime - now;
                    
                    await Task.Delay(delay, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    await _messageBroker.PublishAsync(new FileCleanupBackgroundWorkerStopped(_dateTimeProvider.Now));
                    return;
                }
            }
        }
    }
}