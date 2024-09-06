using Convey;
using Convey.Logging.CQRS;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Communication.Application.Commands;
using System.Reflection;

namespace MiniSpace.Services.Communication.Infrastructure.Logging
{
    internal static class Extensions
    {
        public static IConveyBuilder AddHandlersLogging(this IConveyBuilder builder)
        {
            var assembly = typeof(UpdateMessageStatus).Assembly;

            builder.Services.AddSingleton<IMessageToLogTemplateMapper, MessageToLogTemplateMapper>();

            return builder
                .AddCommandHandlersLogging(assembly)
                .AddEventHandlersLogging(assembly);
        }
    }
}
