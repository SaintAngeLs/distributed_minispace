using Convey;
using Convey.Logging.CQRS;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MiniSpace.Services.Communication.Infrastructure.Logging
{
    internal static class Extensions
    {
        public static IConveyBuilder AddHandlersLogging(this IConveyBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();

            builder.Services.AddSingleton<IMessageToLogTemplateMapper, MessageToLogTemplateMapper>();

            return builder
                .AddCommandHandlersLogging(assembly)
                .AddEventHandlersLogging(assembly);
        }
    }
}
