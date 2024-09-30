using Paralax;
using Paralax.CQRS.Logging;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.MediaFiles.Application.Commands;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Logging
{
    internal static class Extensions
    {
        public static IParalaxBuilder AddHandlersLogging(this IParalaxBuilder builder)
        {
            var assembly = typeof(UploadMediaFile).Assembly;
            
            builder.Services.AddSingleton<IMessageToLogTemplateMapper>(new MessageToLogTemplateMapper());
            
            return builder
                .AddCommandHandlersLogging(assembly)
                .AddEventHandlersLogging(assembly);
        }
    }
}
