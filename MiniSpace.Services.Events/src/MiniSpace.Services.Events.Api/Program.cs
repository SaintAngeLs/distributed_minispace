using System;
using System.Threading.Tasks;
using Convey;
using Convey.Logging;
using Convey.Types;
using Convey.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
//using MiniSpace.Services.Events.Application;
//using MiniSpace.Services.Events.Application.Commands;
//using MiniSpace.Services.Events.Application.Queries;
//using MiniSpace.Services.Events.Application.Services;
//using MiniSpace.Services.Events.Infrastructure;

namespace MiniSpace.Services.Identity.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddConvey()
                    .AddWebApi()
                    //.AddApplication()
                    //.AddInfrastructure()
                    .Build())
                .Configure(app => app
                    //.UseInfrastructure()
                    .UseEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                    ))
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
