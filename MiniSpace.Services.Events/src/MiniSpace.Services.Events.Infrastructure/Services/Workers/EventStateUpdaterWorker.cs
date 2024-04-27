using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.Persistence.MongoDB;
using Microsoft.Extensions.Hosting;
using MiniSpace.Services.Events.Application.Commands;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Events.Infrastructure.Services.Workers
{
    public class EventStateUpdaterWorker: BackgroundService
    {
        private readonly IMessageBroker _messageBroker;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IDateTimeProvider _dateTimeProvider;
        
        public EventStateUpdaterWorker(IMessageBroker messageBroker, ICommandDispatcher commandDispatcher,
            IDateTimeProvider dateTimeProvider)
        {
            _messageBroker = messageBroker;
            _commandDispatcher = commandDispatcher;
            _dateTimeProvider = dateTimeProvider;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _messageBroker.PublishAsync(new EventBackgroundWorkerStarted("state_updater"));
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = _dateTimeProvider.Now;
                    var minutes = now.Minute;
                    if (minutes % 10 == 0)
                    {
                        await _commandDispatcher.SendAsync(new UpdateEventsState(now), stoppingToken);
                    }
                    
                    var nextTime = now.AddMinutes(10 - (minutes % 10));
                    var delay = nextTime - now;
                    
                    await Task.Delay(delay, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    await _messageBroker.PublishAsync(new EventBackgroundWorkerStopped("state_updater"));
                    return;
                }
            }
        }
    }
}