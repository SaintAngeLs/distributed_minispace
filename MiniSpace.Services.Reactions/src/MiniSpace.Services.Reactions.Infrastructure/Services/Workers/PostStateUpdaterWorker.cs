using Convey.CQRS.Commands;
using Microsoft.Extensions.Hosting;
using MiniSpace.Services.Reactions.Application.Commands;
using MiniSpace.Services.Reactions.Application.Events;
using MiniSpace.Services.Reactions.Application.Services;

namespace MiniSpace.Services.Reactions.Infrastructure.Services.Workers
{
    // public class PostStateUpdaterWorker: BackgroundService
    // {
    //     private readonly IMessageBroker _messageBroker;
    //     private readonly ICommandDispatcher _commandDispatcher;
    //     private readonly IDateTimeProvider _dateTimeProvider;
    //     private readonly TimeSpan _updateInterval = TimeSpan.FromMinutes(10);

    //     public PostStateUpdaterWorker(IMessageBroker messageBroker, ICommandDispatcher commandDispatcher,
    //         IDateTimeProvider dateTimeProvider)
    //     {
    //         _messageBroker = messageBroker;
    //         _commandDispatcher = commandDispatcher;
    //         _dateTimeProvider = dateTimeProvider;
    //     }

    //     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //     {
    //         await _messageBroker.PublishAsync(new PostBackgroundWorkerStarted("state_updater"));
    //         while (!stoppingToken.IsCancellationRequested)
    //         {
    //             try
    //             {
    //                 await _commandDispatcher.SendAsync(new UpdatePostsState(_dateTimeProvider.Now), stoppingToken);
    //                 await Task.Delay(_updateInterval, stoppingToken);
    //             }
    //             catch (TaskCanceledException)
    //             {
    //                 await _messageBroker.PublishAsync(new PostBackgroundWorkerStopped("state_updater"));
    //                 return;
    //             }
    //         }
    //     }
    // }
}
