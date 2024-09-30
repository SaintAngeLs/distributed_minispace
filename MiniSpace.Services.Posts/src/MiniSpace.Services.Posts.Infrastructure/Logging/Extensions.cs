using System.Diagnostics.CodeAnalysis;
using Paralax;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Posts.Application.Commands;
using Paralax.CQRS.Logging;

namespace MiniSpace.Services.Posts.Infrastructure.Logging
{
    [ExcludeFromCodeCoverage]
    internal static class Extensions
    {
        public static IParalaxBuilder AddHandlersLogging(this IParalaxBuilder builder)
        {
            var assembly = typeof(UpdatePost).Assembly;
            
            builder.Services.AddSingleton<IMessageToLogTemplateMapper>(new MessageToLogTemplateMapper());
            
            return builder
                .AddCommandHandlersLogging(assembly)
                .AddEventHandlersLogging(assembly);
        }
    }   
}
