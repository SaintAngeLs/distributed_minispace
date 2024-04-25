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
using MiniSpace.Services.Organizations.Application;
using MiniSpace.Services.Organizations.Infrastructure;

namespace MiniSpace.Services.Organizations.Api
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
                        //.Get<GetUserOrganizations, IEnumerable<>>("organizations/user")
                        //.Get<GetRootOrganizations, IEnumerable<>>("organizations/root")
                        //.Get<GetChildrenOrganizations, IEnumerable<>>("organizations/{organizationId}/children")
                        //.Get<GetOrganizers, IEnumerable<>>("organizers")
                        //.Post<AddOrganization>("organizations",
                        //    afterDispatch: (cmd, ctx) => ctx.Response.Created($"organizations/{cmd.OrganizationId}"))
                        //.Post<AddUserToOrganization>("organizations/user")
                        //.Delete<RemoveUserFromOrganization>("organizations/user")
                        ))
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
