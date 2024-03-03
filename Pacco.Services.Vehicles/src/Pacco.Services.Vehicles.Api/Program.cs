using System.Threading.Tasks;
using Convey;
using Convey.Secrets.Vault;
using Convey.CQRS.Queries;
using Convey.Logging;
using Convey.Types;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.Vehicles.Application;
using Pacco.Services.Vehicles.Application.Commands;
using Pacco.Services.Vehicles.Application.DTO;
using Pacco.Services.Vehicles.Application.Queries;
using Pacco.Services.Vehicles.Infrastructure;

namespace Pacco.Services.Vehicles.Api
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
                        .Get<GetVehicle, VehicleDto>("vehicles/{vehicleId}")
                        .Get<SearchVehicles, PagedResult<VehicleDto>>("vehicles")
                        .Post<AddVehicle>("vehicles",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"vehicles/{cmd.VehicleId}"))
                        .Put<UpdateVehicle>("vehicles/{vehicleId}")
                        .Delete<DeleteVehicle>("vehicles/{vehicleId}")
                    ))
                .UseLogging()
                .UseVault()
                .Build()
                .RunAsync();
    }
}
