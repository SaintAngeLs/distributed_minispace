using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Comments;

namespace MiniSpace.Services.Comments.Application
{
    public static class Extensions
    {
        public static IConveyBuilder AddApplication(this IConveyBuilder builder)
            => builder
                .AddCommandHandlers()
                .AddEventHandlers()
                .AddInMemoryCommandDispatcher()
                .AddInMemoryEventDispatcher();
    }
}