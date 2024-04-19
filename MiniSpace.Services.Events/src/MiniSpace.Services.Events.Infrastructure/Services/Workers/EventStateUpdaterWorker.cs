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
        private readonly TimeSpan _updateInterval = TimeSpan.FromMinutes(10);
        
        public EventStateUpdaterWorker(IMessageBroker messageBroker, ICommandDispatcher commandDispatcher)
        {
            _messageBroker = messageBroker;
            _commandDispatcher = commandDispatcher;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _messageBroker.PublishAsync(new EventBackgroundWorkerStarted("state_updater"));
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _commandDispatcher.SendAsync(new UpdateEventsState(), stoppingToken);
                    await Task.Delay(_updateInterval, stoppingToken);
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