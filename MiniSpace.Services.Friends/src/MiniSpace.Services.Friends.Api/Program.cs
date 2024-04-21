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
using MiniSpace.Services.Friends.Application;
using MiniSpace.Services.Friends.Application.Commands;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Application.Queries;
using MiniSpace.Services.Friends.Infrastructure;

namespace MiniSpace.Services.Friends.Api
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
                        .Get<GetStudents, IEnumerable<StudentDto>>("students")
                        .Get<GetStudent, StudentDto>("students/{studentId}")
                        .Put<UpdateStudent>("students/{studentId}")
                        .Delete<DeleteStudent>("students/{studentId}")
                        .Post<CompleteStudentRegistration>("students",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"students/{cmd.StudentId}"))
                        .Put<ChangeStudentState>("students/{studentId}/state/{state}",
                            afterDispatch: (cmd, ctx) => ctx.Response.NoContent())
                        .Get<GetStudentEvents, StudentEventsDto>("students/{studentId}/events")))
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
