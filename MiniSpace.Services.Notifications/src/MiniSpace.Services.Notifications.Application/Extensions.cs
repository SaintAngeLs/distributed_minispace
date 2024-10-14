using Paralax;
using Paralax.CQRS.Commands;
using Paralax.CQRS.Events;

namespace MiniSpace.Services.Notifications.Application
{
    public static class Extensions
    {
        public static IParalaxBuilder AddApplication(this IParalaxBuilder builder)
            => builder
                .AddCommandHandlers()
                .AddEventHandlers()
                .AddInMemoryCommandDispatcher()
                .AddInMemoryEventDispatcher();
    }    
}
