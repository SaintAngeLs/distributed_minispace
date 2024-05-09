using Convey.CQRS.Commands;
using Microsoft.Extensions.Hosting;
using MiniSpace.Services.Posts.Application.Commands;
using MiniSpace.Services.Posts.Application.Events;
using MiniSpace.Services.Posts.Application.Services;

namespace MiniSpace.Services.Posts.Infrastructure.Services.Workers
{
    public class PostStateUpdaterWorker: BackgroundService
    {
        private readonly IMessageBroker _messageBroker;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IDateTimeProvider _dateTimeProvider;
        private const int MinutesInterval = 5;

        public PostStateUpdaterWorker(IMessageBroker messageBroker, ICommandDispatcher commandDispatcher,
            IDateTimeProvider dateTimeProvider)
        {
            _messageBroker = messageBroker;
            _commandDispatcher = commandDispatcher;
            _dateTimeProvider = dateTimeProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _messageBroker.PublishAsync(new PostBackgroundWorkerStarted("state_updater"));
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = _dateTimeProvider.Now;
                    var minutes = now.Minute;
                    if(minutes % MinutesInterval == 0)
                    {
                        await _commandDispatcher.SendAsync(new UpdatePostsState(now), stoppingToken);
                    }
                    
                    var nextTime = now.AddMinutes(MinutesInterval - (minutes % MinutesInterval)).AddSeconds(-now.Second)
                        .AddMilliseconds(-now.Millisecond);
                    var delay = nextTime - now;
                    
                    await Task.Delay(delay, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    await _messageBroker.PublishAsync(new PostBackgroundWorkerStopped("state_updater"));
                    return;
                }
            }
        }
    }
}
