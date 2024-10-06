using System.Diagnostics.CodeAnalysis;
using Paralax;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Reactions.Application.Commands;
using Paralax.CQRS.Logging;

namespace MiniSpace.Services.Reactions.Infrastructure.Logging
{
    [ExcludeFromCodeCoverage]
    internal static class Extensions
    {
        public static IParalaxBuilder AddHandlersLogging(this IParalaxBuilder builder)
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
