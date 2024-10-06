using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MiniSpace.Services.Events.Application.Commands;
using Paralax.CQRS.Commands;
using MiniSpace.Services.Events.Application.Services;

namespace MiniSpace.Services.Events.Infrastructure.Services.Workers
{
    [ExcludeFromCodeCoverage]
    public class EventPublisherWorker : BackgroundService
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IDateTimeProvider _dateTimeProvider;
        private const int MinutesInterval = 5;

        public EventPublisherWorker(ICommandDispatcher commandDispatcher, IDateTimeProvider dateTimeProvider)
        {
            _commandDispatcher = commandDispatcher;
            _dateTimeProvider = dateTimeProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = _dateTimeProvider.Now;
                if (now.Minute % MinutesInterval == 0)
                {
                    await _commandDispatcher.SendAsync(new PublishEvents(now), stoppingToken);
                }

                var delay = GetNextIntervalDelay(now);
                await Task.Delay(delay, stoppingToken);
            }
        }

        private TimeSpan GetNextIntervalDelay(DateTime now)
        {
            var nextTime = now.AddMinutes(MinutesInterval - (now.Minute % MinutesInterval))
                               .AddSeconds(-now.Second)
                               .AddMilliseconds(-now.Millisecond);
            return nextTime - now;
        }
    }
}
