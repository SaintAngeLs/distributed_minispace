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
using Pacco.Services.Customers.Application;
using Pacco.Services.Customers.Application.Commands;
using Pacco.Services.Customers.Application.DTO;
using Pacco.Services.Customers.Application.Queries;
using Pacco.Services.Customers.Infrastructure;

namespace Pacco.Services.Customers.Api
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
                        .Get<GetCustomers, IEnumerable<CustomerDto>>("customers")
                        .Get<GetCustomer, CustomerDetailsDto>("customers/{customerId}")
                        .Get<GetCustomerState, CustomerStateDto>("customers/{customerId}/state")
                        .Post<CompleteCustomerRegistration>("customers",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"customers/{cmd.CustomerId}"))
                        .Put<ChangeCustomerState>("customers/{customerId}/state/{state}",
                            afterDispatch: (cmd, ctx) => ctx.Response.NoContent())))
                .UseLogging()
                .UseVault()
                .Build()
                .RunAsync();
    }
}