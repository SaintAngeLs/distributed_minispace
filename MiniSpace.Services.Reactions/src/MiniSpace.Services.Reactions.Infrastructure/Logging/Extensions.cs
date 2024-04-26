using Convey;
using Convey.Logging.CQRS;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Reactions.Application.Commands;

namespace MiniSpace.Services.Reactions.Infrastructure.Logging
{
    // internal static class Extensions
    // {
    //     public static IConveyBuilder AddHandlersLogging(this IConveyBuilder builder)
    //     {
    //         var assembly = typeof(UpdatePost).Assembly;
            
    //         builder.Services.AddSingleton<IMessageToLogTemplateMapper>(new MessageToLogTemplateMapper());
            
    //         return builder
    //             .AddCommandHandlersLogging(assembly)
    //             .AddEventHandlersLogging(assembly);
    //     }
    // }   
}
