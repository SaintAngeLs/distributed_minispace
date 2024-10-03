using Paralax;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Students.Application.Commands;
using System.Diagnostics.CodeAnalysis;
using Paralax.CQRS.Logging;

namespace MiniSpace.Services.Students.Infrastructure.Logging
{
    [ExcludeFromCodeCoverage]
    internal static class Extensions
    {
        public static IParalaxBuilder AddHandlersLogging(this IParalaxBuilder builder)
        {
            var assembly = typeof(UpdateStudent).Assembly;
            
            builder.Services.AddSingleton<IMessageToLogTemplateMapper>(new MessageToLogTemplateMapper());
            
            return builder
                .AddCommandHandlersLogging(assembly)
                .AddEventHandlersLogging(assembly);
        }
    }
}
