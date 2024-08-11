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
                        .Get<GetOrganizationDetails, OrganizationDetailsDto>("organizations/{organizationId}/details")
                        .Get<GetRootOrganizations, IEnumerable<OrganizationDto>>("organizations/root")
                        .Get<GetChildrenOrganizations, PagedResult<OrganizationDto>>("organizations/{organizationId}/children")
                        .Get<GetAllChildrenOrganizations, IEnumerable<Guid>>("organizations/{organizationId}/children/all")
                        .Get<GetUserOrganizations, IEnumerable<UserOrganizationsDto>>("users/{userId}/organizations")
                        .Get<GetOrganizationWithGalleryAndUsers, OrganizationGalleryUsersDto>("organizations/{organizationId}/details/gallery-users")
                        .Get<GetOrganizationRoles, IEnumerable<RoleDto>>("organizations/{organizationId}/roles")
                        .Get<GetPaginatedOrganizations, PagedResult<OrganizationDto>>("organizations/paginated")
                         
                        .Post<CreateOrganization>("organizations",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"organizations/{cmd.OrganizationId}"))
                        .Post<CreateSubOrganization>("organizations/{organizationId}/children",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"organizations/{cmd.SubOrganizationId}"))
                        .Post<CreateOrganizationRole>("organizations/{organizationId}/roles",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"organizations/{cmd.OrganizationId}/roles/{cmd.RoleName}"))
                        .Post<FollowOrganization>("organizations/{organizationId}/follow", 
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"organizations/{cmd.OrganizationId}/follow"))
                        .Delete<DeleteOrganization>("organizations/{organizationId}")
                        .Post<InviteUserToOrganization>("organizations/{organizationId}/invite")
                        .Post<AssignRoleToMember>("organizations/{organizationId}/roles/{memberId}")
                        .Put<UpdateRolePermissions>("organizations/{organizationId}/roles/{roleId}/permissions")
                        .Post<SetOrganizationPrivacy>("organizations/{organizationId}/privacy")
                        .Put<UpdateOrganizationSettings>("organizations/{organizationId}/settings")
                        .Put<SetOrganizationVisibility>("organizations/{organizationId}/visibility")
                        .Put<ManageFeed>("organizations/{organizationId}/feed")
                        .Put<UpdateOrganization>("organizations/{organizationId}",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"organizations/{cmd.OrganizationId}"))
                        ))
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
