using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.Parcels.Core.Services;

namespace Pacco.Services.Parcels.Application
{
    public static class Extensions
    {
        public static IConveyBuilder AddApplication(this IConveyBuilder builder)
        {
            builder
                .AddCommandHandlers()
                .AddEventHandlers()
                .AddInMemoryCommandDispatcher()
                .AddInMemoryEventDispatcher();

            builder.Services.AddSingleton<IParcelsService>(new ParcelsService());

            return builder;
        }
    }
}