using System.Collections.Generic;
using System.Threading.Tasks;
using Paralax;
using Paralax.Logging;
using Paralax.Types;
using Paralax.WebApi;
using Paralax.CQRS.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Email.Application;
using MiniSpace.Services.Email.Application.Commands;
using MiniSpace.Services.Email.Application.Dto;
using MiniSpace.Services.Email.Application.Queries;
using MiniSpace.Services.Email.Infrastructure;
using Paralax.Types;


namespace MiniSpace.Services.Email.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddParalax()
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
