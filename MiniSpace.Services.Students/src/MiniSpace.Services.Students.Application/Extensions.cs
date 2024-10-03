using Paralax;
using Paralax.CQRS.Commands;
using Paralax.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application
{
    [ExcludeFromCodeCoverage]
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
