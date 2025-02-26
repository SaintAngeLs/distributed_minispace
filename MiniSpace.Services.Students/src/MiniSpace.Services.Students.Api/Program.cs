using System.Collections.Generic;
using System.Threading.Tasks;
using Paralax;
using Paralax.CQRS.Queries;
using Paralax.Logging;
using Paralax.Types;
using Paralax.WebApi;
using Paralax.CQRS.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Students.Api.Grpc;
using MiniSpace.Services.Students.Application;
using MiniSpace.Services.Students.Application.Commands;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Core.Wrappers;
using MiniSpace.Services.Students.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Paralax.CQRS.WebApi;
using Paralax.Core;

namespace MiniSpace.Services.Students.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => 
                {
                    services.AddParalax()
                            .AddWebApi()
                            .AddApplication()
                            .AddInfrastructure();
                    
                    services.AddGrpc();
                    
                    services.AddCors(options =>
                    {
                        options.AddPolicy("CorsPolicy",
                            builder => builder
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials()
                                .SetIsOriginAllowed(host => true));
                    });
                    
                    services.AddSignalR();
                })
                .Configure(app => app
                    .UseInfrastructure()
                    .UseRouting()
                    .UseCors("CorsPolicy")
                    .UseEndpoints(endpoints =>
                    {
                        // endpoints.MapGrpcService<StudentServiceGrpc>();
                        endpoints.MapHub<MiniSpace.Services.Students.Application.Hubs.PresenceHub>("/presenceHub")
                            .RequireCors("CorsPolicy");
                    })
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetUsers, Application.Queries.PagedResult<UserDto>>("students")
                        .Get<GetUser, UserDto>("students/{userId}")
                        .Get<GetUserSettings, UserSettingsDto>("students/{userId}/settings")
                        .Get<GetUserWithGalleryImages, StudentWithGalleryImagesDto>("students/{userId}/gallery")
                        .Get<GetUserWithVisibilitySettings, StudentWithVisibilitySettingsDto>("students/{studentId}/visibility-settings")
                        .Get<GetUserEvents, UserEventsDto>("students/{studentId}/events")
                        .Get<GetUserNotificationPreferences, NotificationPreferencesDto>("students/{userId}/notifications")
                        .Get<GetUserProfileViews, PagedResponse<UserProfileViewDto>>("students/profiles/users/{userId}/views/paginated")
                        .Get<GetProfilesViewedByUser, PagedResponse<UserProfileViewDto>>("students/profiles/users/{userId}/views/viewed")
                        .Get<GetBlockedUsers, PagedResponse<BlockedUserDto>>("students/{blockerId}/blocked-users")

                        .Put<UpdateUser>("students/{studentId}")
                        .Put<UpdateUserSettings>("students/{studentId}/settings")
                        .Put<ChangeUserState>("students/{studentId}/state/{state}",
                            afterDispatch: (cmd, ctx) => ctx.Response.NoContent())
                        .Put<UpdateUserLanguagesAndInterests>("students/{studentId}/languages-and-interests")

                        .Delete<DeleteUser>("students/{studentId}")

                        .Post<BlockUser>("students/{blockerId}/block-user/{blockedUserId}",
                            afterDispatch: (cmd, ctx) => ctx.Response.Ok())
                        .Post<UnblockUser>("students/{blockerId}/unblock-user/{blockedUserId}",
                            afterDispatch: (cmd, ctx) => ctx.Response.Ok())

                        .Post<CompleteUserRegistration>("students",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"students/{cmd.UserId}"))
                        .Post<UpdateUserNotificationPreferences>("students/{studentId}/notifications")
                        .Post<ViewUserProfile>("students/profiles/users/{userProfileId}/view", afterDispatch: (cmd, ctx) => ctx.Response.Ok())
                    )
                
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapGrpcService<StudentServiceGrpc>(); 
                }))
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
