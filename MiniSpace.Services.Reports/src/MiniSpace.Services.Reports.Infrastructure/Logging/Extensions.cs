using Paralax;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Reports.Application.Commands;
using Paralax.CQRS.Logging;

namespace MiniSpace.Services.Reports.Infrastructure.Logging
{
    internal static class Extensions
    {
        public static IParalaxBuilder AddHandlersLogging(this IParalaxBuilder builder)
        {
            var assembly = typeof(CreateReport).Assembly;
            
            builder.Services.AddSingleton<IMessageToLogTemplateMapper>(new MessageToLogTemplateMapper());
            
            return builder
                .AddCommandHandlersLogging(assembly)
                .AddEventHandlersLogging(assembly);
        }
    }
}
