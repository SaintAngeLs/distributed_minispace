using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Paralax.CQRS.Commands;
using Paralax.Persistence.MongoDB;
using Microsoft.Extensions.Hosting;
using MiniSpace.Services.Events.Application.Commands;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Events.Infrastructure.Services.Workers
{
    [ExcludeFromCodeCoverage]
    public class EventStateUpdaterWorker: BackgroundService
    {
        private readonly IMessageBroker _messageBroker;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IDateTimeProvider _dateTimeProvider;
        private const int MinutesInterval = 5;
        
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
                    if (minutes % MinutesInterval == 0)
                    {
                        await _commandDispatcher.SendAsync(new UpdateEventsState(now), stoppingToken);
                    }
                    
                    var nextTime = now.AddMinutes(MinutesInterval - (minutes % MinutesInterval)).AddSeconds(-now.Second)
                        .AddMilliseconds(-now.Millisecond);
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