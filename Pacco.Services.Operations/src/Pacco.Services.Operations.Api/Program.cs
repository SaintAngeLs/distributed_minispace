using System.Threading.Tasks;
using Convey;
using Convey.Secrets.Vault;
using Convey.Logging;
using Convey.Types;
using Convey.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.Operations.Api.Hubs;
using Pacco.Services.Operations.Api.Infrastructure;
using Pacco.Services.Operations.Api.Queries;
using Pacco.Services.Operations.Api.Services;

namespace Pacco.Services.Operations.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddConvey()
                    .AddWebApi()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    .UseInfrastructure()
                    .UseEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetOperation>("operations/{operationId}", async (query, ctx) =>
                        {
                            var operation = await ctx.RequestServices.GetService<IOperationsService>()
                                .GetAsync(query.OperationId);
                            if (operation is null)
                            {
                                await ctx.Response.NotFound();
                                return;
                            }

                            await ctx.Response.WriteJsonAsync(operation);
                        }))
                    .UseEndpoints(endpoints =>
                    {
                        endpoints.MapHub<PaccoHub>("/pacco");
                        endpoints.MapGrpcService<GrpcServiceHost>();
                    }))
                .UseLogging()
                .UseVault()
                .Build()
                .RunAsync();
    }
}
