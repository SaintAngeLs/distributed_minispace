using System.Collections.Generic;
using System.Threading.Tasks;
using Paralax;
using Paralax.Logging;
using Paralax.Types;
using Paralax.WebApi;
using Paralax.CQRS.WebApi;
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
using MiniSpace.Services.Notifications.Application.Hubs;


namespace MiniSpace.Services.Notifications.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddParalax()
                            .AddWebApi()
                            .AddApplication()
                            .AddInfrastructure();
                    services.AddCors(options =>
                    {
                        options.AddPolicy("CorsPolicy",
                            builder => builder
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .SetIsOriginAllowed((host) => true));
                    });
                    services.AddSignalR();
                    services.AddAuthentication(); 
                    services.AddAuthorization(); 
                })
                .Configure(app => app
                    .UseInfrastructure()
                    .UseRouting()
                    .UseCors("CorsPolicy")
                    .UseAuthentication()
                    .UseAuthorization()
                    .UseEndpoints(endpoints =>
                    {
                        endpoints.MapHub<NotificationHub>("/notificationHub").RequireCors("CorsPolicy");
                        endpoints.MapHub<ChatHub>("/chatHub").RequireCors("CorsPolicy");
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
