using System.Threading.Tasks;
using Convey;
using Convey.Secrets.Vault;
using Convey.Logging;
using Convey.Types;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.Deliveries.Application;
using Pacco.Services.Deliveries.Application.Commands;
using Pacco.Services.Deliveries.Application.DTO;
using Pacco.Services.Deliveries.Application.Queries;
using Pacco.Services.Deliveries.Infrastructure;

namespace Pacco.Services.Deliveries.Api
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
                        .Get<GetDelivery, DeliveryDto>("deliveries/{deliveryId}")
                        .Post<StartDelivery>("deliveries",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"deliveries/{cmd.DeliveryId}"))
                        .Post<FailDelivery>("deliveries/{deliveryId}/fail")
                        .Post<CompleteDelivery>("deliveries/{deliveryId}/complete")
                        .Post<AddDeliveryRegistration>("deliveries/{deliveryId}/registrations")))
                .UseLogging()
                .UseVault()
                .Build()
                .RunAsync();
    }
}
