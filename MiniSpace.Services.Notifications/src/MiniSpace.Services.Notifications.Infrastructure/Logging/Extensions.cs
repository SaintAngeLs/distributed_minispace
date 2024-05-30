using Convey;
using Convey.Logging.CQRS;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Notifications.Application.Commands;

namespace MiniSpace.Services.Notifications.Infrastructure.Logging
{
    internal static class Extensions
    {
        public static IConveyBuilder AddHandlersLogging(this IConveyBuilder builder)
        {
            var assembly = typeof(UpdateNotificationStatus).Assembly;
            
            // Modify this line to let the DI container handle the instantiation and provide the necessary logger
            builder.Services.AddSingleton<IMessageToLogTemplateMapper, MessageToLogTemplateMapper>();
            
            return builder
                .AddCommandHandlersLogging(assembly)
                .AddEventHandlersLogging(assembly);
        }
    }
}
