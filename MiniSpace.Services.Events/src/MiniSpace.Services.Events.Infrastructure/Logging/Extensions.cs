using Paralax;
using Paralax.CQRS.Logging;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Events.Application.Commands;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Infrastructure.Logging
{
    [ExcludeFromCodeCoverage]
    internal static class Extensions
    {
        public static IParalaxBuilder AddHandlersLogging(this IParalaxBuilder builder)
        {
            var assembly = typeof(DeleteEvent).Assembly;
            
            builder.Services.AddSingleton<IMessageToLogTemplateMapper>(new MessageToLogTemplateMapper());
            
            return builder
                .AddCommandHandlersLogging(assembly)
                .AddEventHandlersLogging(assembly);
        }
    }
}