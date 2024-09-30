using System.Collections.Generic;
using System.Threading.Tasks;
using Paralax;
using Paralax.CQRS.Queries;
using Paralax.Logging;
using Paralax.Types;
using Paralax.WebApi;
using Paralax.WebApi.CQRS;
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
                })
                .Configure(app => app
                    .UseInfrastructure()
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetStudents, Application.Queries.PagedResult<StudentDto>>("students")
                        .Get<GetStudent, StudentDto>("students/{studentId}")
                        .Get<GetUserSettings, UserSettingsDto>("students/{studentId}/settings")
                        .Get<GetStudentWithGalleryImages, StudentWithGalleryImagesDto>("students/{studentId}/gallery")
                        .Get<GetStudentWithVisibilitySettings, StudentWithVisibilitySettingsDto>("students/{studentId}/visibility-settings")
                        .Get<GetStudentEvents, StudentEventsDto>("students/{studentId}/events")
                        .Get<GetUserNotificationPreferences, NotificationPreferencesDto>("students/{studentId}/notifications")
                        .Get<GetUserProfileViews, PagedResponse<UserProfileViewDto>>("students/profiles/users/{userId}/views/paginated")
                        .Get<GetProfilesViewedByUser, PagedResponse<UserProfileViewDto>>("students/profiles/users/{userId}/views/viewed")
                        .Get<GetBlockedUsers, PagedResponse<BlockedUserDto>>("students/{blockerId}/blocked-users")

                        .Put<UpdateStudent>("students/{studentId}")
                        .Put<UpdateUserSettings>("students/{studentId}/settings")
                        .Put<ChangeStudentState>("students/{studentId}/state/{state}",
                            afterDispatch: (cmd, ctx) => ctx.Response.NoContent())
                        .Put<UpdateStudentLanguagesAndInterests>("students/{studentId}/languages-and-interests")

                        .Delete<DeleteStudent>("students/{studentId}")

                        .Post<BlockUser>("students/{blockerId}/block-user/{blockedUserId}",
                            afterDispatch: (cmd, ctx) => ctx.Response.Ok())
                        .Post<UnblockUser>("students/{blockerId}/unblock-user/{blockedUserId}",
                            afterDispatch: (cmd, ctx) => ctx.Response.Ok())

                        .Post<CompleteStudentRegistration>("students",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"students/{cmd.StudentId}"))
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
