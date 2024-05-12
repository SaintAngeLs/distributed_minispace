using System.Collections.Generic;
using System.Threading.Tasks;
using Convey;
using Convey.Logging;
using Convey.Types;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Notifications.Application;
using MiniSpace.Services.Notifications.Application.Commands;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Queries;
using MiniSpace.Services.Notifications.Infrastructure;

namespace MiniSpace.Services.Notifications.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddConvey()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    .UseInfrastructure()
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetUserNotifications, IEnumerable<NotificationDto>>("notifications/{userId}", afterDispatch: (query, ctx) => ctx.Response.Ok())
                        .Post<CreateNotification>("notifications", afterDispatch: (cmd, ctx) => ctx.Response.Created($"notifications/{cmd.NotificationId}")))
                .UseLogging()
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
