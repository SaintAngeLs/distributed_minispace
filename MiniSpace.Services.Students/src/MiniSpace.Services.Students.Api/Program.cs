using System.Collections.Generic;
using System.Threading.Tasks;
using Convey;
using Convey.CQRS.Queries;
using Convey.Logging;
using Convey.Types;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Students.Application;
using MiniSpace.Services.Students.Application.Commands;
using MiniSpace.Services.Students.Application.Dto;
using MiniSpace.Services.Students.Application.Queries;
using MiniSpace.Services.Students.Infrastructure;

namespace MiniSpace.Services.Students.Api
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
                        .Get<GetStudents, Application.Queries.PagedResult<StudentDto>>("students")
                        .Get<GetStudent, StudentDto>("students/{studentId}")
                        .Get<GetUserSettings, UserSettingsDto>("students/{studentId}/settings")
                        .Get<GetStudentWithGalleryImages, StudentWithGalleryImagesDto>("students/{studentId}/gallery")
                        .Get<GetStudentWithVisibilitySettings, StudentWithVisibilitySettingsDto>("students/{studentId}/visibility-settings")
                        .Get<GetStudentEvents, StudentEventsDto>("students/{studentId}/events")
                        .Get<GetUserNotificationPreferences, NotificationPreferencesDto>("students/{studentId}/notifications")

                        .Put<UpdateStudent>("students/{studentId}")
                        .Put<UpdateUserSettings>("students/{studentId}/settings")
                        .Put<ChangeStudentState>("students/{studentId}/state/{state}",
                            afterDispatch: (cmd, ctx) => ctx.Response.NoContent())
                        .Put<UpdateStudentLanguagesAndInterests>("students/{studentId}/languages-and-interests")

                        .Delete<DeleteStudent>("students/{studentId}")

                        .Post<CompleteStudentRegistration>("students",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"students/{cmd.StudentId}"))  
                        .Post<UpdateUserNotificationPreferences>("students/{studentId}/notifications")
                        
                        ))
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
