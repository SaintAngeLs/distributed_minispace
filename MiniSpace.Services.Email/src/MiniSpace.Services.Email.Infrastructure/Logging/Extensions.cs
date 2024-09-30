using Paralax;
using Paralax.Logging.CQRS;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Email.Application.Commands;

namespace MiniSpace.Services.Email.Infrastructure.Logging
{
    internal static class Extensions
    {
        public static IParalaxBuilder AddHandlersLogging(this IParalaxBuilder builder)
        {
            var assembly = typeof(CreateEmailNotification).Assembly;
            
            builder.Services.AddSingleton<IMessageToLogTemplateMapper>(new MessageToLogTemplateMapper());
            
            return builder
                .AddCommandHandlersLogging(assembly)
                .AddEventHandlersLogging(assembly);
        }
    }
}
