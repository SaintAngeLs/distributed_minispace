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
using MiniSpace.Services.Email.Application;
using MiniSpace.Services.Email.Application.Commands;
using MiniSpace.Services.Email.Application.Dto;
using MiniSpace.Services.Email.Application.Queries;
using MiniSpace.Services.Email.Infrastructure;

namespace MiniSpace.Services.Email.Api
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
                        .Get<GetEmailNotificationsByUser, Application.Queries.PagedResult<EmailNotificationDto>>("email-notifications/{userId}")
                        .Post<CreateEmailNotification>("email-notifications",
                            afterDispatch: (cmd, ctx) => ctx.Response.WriteAsJsonAsync(new { Message = "Email notification created successfully.", NotificationId = cmd.EmailNotificationId }))))
                .UseLogging()
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
