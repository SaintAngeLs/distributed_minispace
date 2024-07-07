using System.Collections.Generic;
using System.Threading.Tasks;
using Convey;
using Convey.Logging;
using Convey.Types;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Notifications.Application;
using MiniSpace.Services.Notifications.Application.Commands;
using MiniSpace.Services.Notifications.Application.Dto;
using MiniSpace.Services.Notifications.Application.Queries;
using MiniSpace.Services.Notifications.Infrastructure;
using MiniSpace.Services.Notifications.Infrastructure.Hubs;

namespace MiniSpace.Services.Notifications.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddConvey()
                            .AddWebApi()
                            .AddApplication()
                            .AddInfrastructure();
                    services.AddSignalR(); 
                })
                .Configure(app => app
                    .UseInfrastructure()
                    .UseRouting()
                    .UseEndpoints(endpoints =>
                    {
                        endpoints.MapHub<NotificationHub>("/notificationHub");
                    })
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetNotificationsByUser, Application.Queries.PagedResult<NotificationDto>>("notifications/{userId}")
                        .Get<GetNotification, NotificationDto>("notifications/{userId}/{notificationId}")
                        .Post<CreateNotification>("notifications")
                        .Put<UpdateNotificationStatus>("notifications/{userId}/{notificationId}/status")
                        .Delete<DeleteNotification>("notifications/notification/{userId}/{notificationId}")))
                .UseLogging()
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
