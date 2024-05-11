using Convey;
using Convey.Logging.CQRS;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Reactions.Application.Commands;

namespace MiniSpace.Services.Reactions.Infrastructure.Logging
{
    internal static class Extensions
    {
        public static IConveyBuilder AddHandlersLogging(this IConveyBuilder builder)
        {
            // TODO: Posts had only UpdatePost
            var assemblyCreate = typeof(CreateReaction).Assembly;
            var assemblyDelete = typeof(DeleteReaction).Assembly;
            
            builder.Services.AddSingleton<IMessageToLogTemplateMapper>(new MessageToLogTemplateMapper());
            
            return builder
                .AddCommandHandlersLogging(assemblyCreate)
                .AddCommandHandlersLogging(assemblyDelete)
                .AddEventHandlersLogging(assemblyCreate)
                .AddEventHandlersLogging(assemblyDelete)
                ;
        }
    }   
}
