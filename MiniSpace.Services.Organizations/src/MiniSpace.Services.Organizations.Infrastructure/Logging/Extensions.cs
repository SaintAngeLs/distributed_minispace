using Convey;
using Convey.Logging.CQRS;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Organizations.Application.Commands;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Logging
{
    [ExcludeFromCodeCoverage]
    internal static class Extensions
    {
        public static IConveyBuilder AddHandlersLogging(this IConveyBuilder builder)
        {
            var assembly = typeof(CreateOrganization).Assembly;
            
            builder.Services.AddSingleton<IMessageToLogTemplateMapper>(new MessageToLogTemplateMapper());
            
            return builder
                .AddCommandHandlersLogging(assembly)
                .AddEventHandlersLogging(assembly);
        }
    }
}
