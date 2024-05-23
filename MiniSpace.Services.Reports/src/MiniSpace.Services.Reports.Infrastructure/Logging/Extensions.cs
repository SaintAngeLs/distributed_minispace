using Convey;
using Convey.Logging.CQRS;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Reports.Application.Commands;

namespace MiniSpace.Services.Reports.Infrastructure.Logging
{
    internal static class Extensions
    {
        public static IConveyBuilder AddHandlersLogging(this IConveyBuilder builder)
        {
            var assembly = typeof(CreateReport).Assembly;
            
            builder.Services.AddSingleton<IMessageToLogTemplateMapper>(new MessageToLogTemplateMapper());
            
            return builder
                .AddCommandHandlersLogging(assembly)
                .AddEventHandlersLogging(assembly);
        }
    }
}
