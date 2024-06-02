using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application
{
    [ExcludeFromCodeCoverage]
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
