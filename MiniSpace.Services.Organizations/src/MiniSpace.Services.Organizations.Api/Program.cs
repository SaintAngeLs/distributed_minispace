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
using MiniSpace.Services.Organizations.Application.Commands;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Core.Entities;
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
                        .Get<GetOrganization, OrganizationDto>("organizations/{organizationId}")
                        .Get<GetOrganizerOrganizations, IEnumerable<OrganizationDto>>("organizations/user")
                        .Get<GetRootOrganizations, IEnumerable<OrganizationDto>>("organizations/root")
                        .Get<GetChildrenOrganizations, IEnumerable<OrganizationDto>>("organizations/{organizationId}/children")
                        .Post<AddOrganization>("organizations",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"organizations/root"))
                        //.Post<AddUserToOrganization>("organizations/user")
                        //.Delete<RemoveUserFromOrganization>("organizations/user")
                        ))
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
