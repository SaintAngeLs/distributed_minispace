using System.Collections.Generic;
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
using Pacco.Services.Parcels.Application;
using Pacco.Services.Parcels.Application.Commands;
using Pacco.Services.Parcels.Application.DTO;
using Pacco.Services.Parcels.Application.Queries;
using Pacco.Services.Parcels.Infrastructure;

namespace Pacco.Services.Parcels.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await GetWebHostBuilder(args)
                .Build()
                .RunAsync();

        public static IWebHostBuilder GetWebHostBuilder(string[] args)
            => WebHost.CreateDefaultBuilder(args)
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
                        .Get<GetParcelsVolume, ParcelsVolumeDto>("parcels/volume")
                        .Get<GetParcel, ParcelDto>("parcels/{parcelId}")
                        .Get<GetParcels, IEnumerable<ParcelDto>>("parcels")
                        .Delete<DeleteParcel>("parcels/{parcelId}")
                        .Post<AddParcel>("parcels",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"parcels/{cmd.ParcelId}"))))
                .UseLogging()
                .UseVault();
    }
}
