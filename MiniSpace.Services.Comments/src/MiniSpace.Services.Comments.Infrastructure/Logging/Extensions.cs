using Convey;
using Convey.Logging.CQRS;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Comments.Application.Commands;

namespace MiniSpace.Services.Comments.Infrastructure.Logging
{
    internal static class Extensions
    {
        public static IConveyBuilder AddHandlersLogging(this IConveyBuilder builder)
        {
            var assembly = typeof(UpdateComment).Assembly;
            
            builder.Services.AddSingleton<IMessageToLogTemplateMapper>(new MessageToLogTemplateMapper());
            
            return builder
                .AddCommandHandlersLogging(assembly)
                .AddEventHandlersLogging(assembly);
        }
    }   
}
