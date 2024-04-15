using Convey;
using Convey.Logging.CQRS;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Students.Application.Commands;

namespace MiniSpace.Services.Students.Infrastructure.Logging
{
    internal static class Extensions
    {
        public static IConveyBuilder AddHandlersLogging(this IConveyBuilder builder)
        {
            var assembly = typeof(UpdateStudent).Assembly;
            
            builder.Services.AddSingleton<IMessageToLogTemplateMapper>(new MessageToLogTemplateMapper());
            
            return builder
                .AddCommandHandlersLogging(assembly)
                .AddEventHandlersLogging(assembly);
        }
    }
}
