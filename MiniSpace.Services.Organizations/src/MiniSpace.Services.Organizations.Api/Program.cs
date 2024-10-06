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
using MiniSpace.Services.Organizations.Application;
using MiniSpace.Services.Organizations.Application.Commands;
using MiniSpace.Services.Organizations.Application.DTO;
using MiniSpace.Services.Organizations.Application.Queries;
using MiniSpace.Services.Organizations.Infrastructure;
using Paralax.Core;

namespace MiniSpace.Services.Organizations.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddParalax()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build())
                    
                .Configure(app => app
                    .UseInfrastructure()
                    
                    .UseEndpoints(endpoints => endpoints
                        .Post<CreateOrganization>("organizations", async (cmd, ctx) =>
                        {
                            await ctx.Response.Created($"organizations/{cmd.OrganizationId}");
                        })
                        .Post<CreateSubOrganization>("organizations/{organizationId}/children", async (cmd, ctx) =>
                        {
                            await ctx.Response.Created($"organizations/{cmd.SubOrganizationId}");
                        })
                        .Post<CreateOrganizationRole>("organizations/{organizationId}/roles", async (cmd, ctx) =>
                        {
                            await ctx.Response.Created($"organizations/{cmd.OrganizationId}/roles/{cmd.RoleName}");
                        })
                        .Post<FollowOrganization>("organizations/{organizationId}/follow", async (cmd, ctx) =>
                        {
                            await ctx.Response.Created($"organizations/{cmd.OrganizationId}/follow");
                        })
                        .Post<LeaveOrganization>("organizations/{organizationId}/leave", async (cmd, ctx) =>
                        {
                            await ctx.Response.NoContent();
                        })
                        .Put<AcceptFollowRequest>("organizations/{organizationId}/requests/{requestId}/accept", async (cmd, ctx) =>
                        {
                            await ctx.Response.NoContent();
                        })
                        .Put<RejectFollowRequest>("organizations/{organizationId}/requests/{requestId}/reject", async (cmd, ctx) =>
                        {
                            await ctx.Response.NoContent();
                        })
                        .Delete<DeleteOrganization>("organizations/{organizationId}/delete", async (cmd, ctx) =>
                        {
                            await ctx.Response.NoContent();
                        })
                        
                    )
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetOrganization, OrganizationDto>("organizations/{organizationId}")
                        .Get<GetOrganizationDetails, OrganizationDetailsDto>("organizations/{organizationId}/details")
                        .Get<GetRootOrganizations, IEnumerable<OrganizationDto>>("organizations/root")
                        .Get<GetChildrenOrganizations, PagedResult<OrganizationDto>>("organizations/{organizationId}/children")
                        .Get<GetAllChildrenOrganizations, IEnumerable<Guid>>("organizations/{organizationId}/children/all")
                        .Get<GetPaginatedUserOrganizations, PagedResult<OrganizationDto>>("organizations/users/{userId}/organizations")
                        .Get<GetUserFollowOrganizations, IEnumerable<OrganizationGalleryUsersDto>>("organizations/users/{userId}/organizations/follow")
                        .Get<GetOrganizationWithGalleryAndUsers, OrganizationGalleryUsersDto>("organizations/{organizationId}/details/gallery-users")
                        .Get<GetOrganizationRoles, IEnumerable<RoleDto>>("organizations/{organizationId}/roles")
                        .Get<GetPaginatedOrganizations, PagedResult<OrganizationDto>>("organizations/paginated")
                        .Get<GetPaginatedOrganizationRequests, PagedResult<OrganizationRequestDto>>("organizations/{organizationId}/requests")
                    ))
                .UseLogging()
                .Build()
                .RunAsync();
        }
    }
}
