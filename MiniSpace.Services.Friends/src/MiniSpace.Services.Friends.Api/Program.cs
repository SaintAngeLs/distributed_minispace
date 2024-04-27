using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
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
using MiniSpace.Services.Friends.Application.Events;
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
                        // .Get<GetStudents, IEnumerable<StudentDto>>("students")
                        // .Get<GetStudent, StudentDto>("students/{studentId}")
                        // .Put<UpdateStudent>("students/{studentId}")
                        // .Delete<DeleteStudent>("students/{studentId}")
                        // .Post<CompleteStudentRegistration>("students",
                        //     afterDispatch: (cmd, ctx) => ctx.Response.Created($"students/{cmd.StudentId}"))
                        // .Put<ChangeStudentState>("students/{studentId}/state/{state}",
                        //     afterDispatch: (cmd, ctx) => ctx.Response.NoContent())
                        // .Get<GetStudentEvents, StudentEventsDto>("students/{studentId}/events")))

                        // .Get<IEnumerable<FriendDto>>("friends/{studentId}", 
                        //     ctx => new GetFriends { StudentId = Guid.Parse(ctx.Request.RouteValues["studentId"].ToString()) }, 
                        //     (query, ctx) => ctx.Response.WriteAsJsonAsync(query), // Correctly define delegate with parameters
                        //     afterDispatch: ctx => ctx.Response.Ok())
                        .Get<GetFriends, IEnumerable<FriendDto>>("friends")
                        .Get<GetFriends, IEnumerable<FriendDto>>("friends/{studentId}") 
                        .Get<GetFriendRequests, IEnumerable<FriendRequestDto>>("friends/requests/{studentId}")
                        .Get<PendingFriendAcceptQuery, FriendRequestDto>("friends/requests/{studentId}/accept")
                        .Get<PendingFriendDeclineQuery, FriendRequestDto>("friends/requests/{studentId}/decline")
                        .Get<GetFriends, IEnumerable<FriendDto>>("friends/pending")
                        .Get<GetFriendRequests, IEnumerable<FriendRequestDto>>("friends/pending")
                        .Post<InviteFriend>("friends/{userId}/invite", afterDispatch: (cmd, ctx) => ctx.Response.Created($"friends/{ctx.Request.RouteValues["studentId"]}/invite")))) 
                    //    .Get("friends", async ctx =>
                    //     {
                    //         var query = new GetFriends();
                    //         var result = await ctx.RequestServices.GetRequiredService<IQueryDispatcher>().QueryAsync<IEnumerable<FriendDto>>(query);
                    //         await ctx.Response.WriteAsJsonAsync(result);
                    //     })
//                         .Post("friends/{userId}", async ctx =>
//                         {
//                             var command = new AddFriend(Guid.Parse(ctx.User.Identity.Name), Guid.Parse(ctx.Request.RouteValues["userId"].ToString()));
//                             await ctx.RequestServices.GetRequiredService<ICommandDispatcher>().SendAsync(command);
//                             ctx.Response.Created($"friends/{ctx.Request.RouteValues["userId"]}");
//                         })
//                         .Get("friends/notYet", async ctx =>
//                         {
                            
//                             await ctx.Response.WriteAsJsonAsync(new { Message = "Retrieve not yet confirmed friends." });
//                         })
//                         .Post("friends/pending", async ctx =>
//                         {
//                             var command = new InviteFriend(Guid.Parse(ctx.User.Identity.Name), Guid.NewGuid()); 
//                             await ctx.RequestServices.GetRequiredService<ICommandDispatcher>().SendAsync(command);
//                             ctx.Response.Created("friends/pending");
//                         })
//                         .Get("friends/pending", async ctx =>
//                         {
//                             var query = new GetFriendRequests(Guid.Parse(ctx.User.Identity.Name));
//                             var result = await ctx.RequestServices.GetRequiredService<IQueryDispatcher>().QueryAsync<IEnumerable<FriendRequestDto>>(query);
//                             await ctx.Response.WriteAsJsonAsync(result);
//                         })
//                         .Post("friends/{userId}/invite", async ctx =>
// {
//     var userIdRouteValue = ctx.Request.RouteValues["userId"]?.ToString();
//     Console.WriteLine($"Received userId: {userIdRouteValue}");

//     if (string.IsNullOrEmpty(userIdRouteValue))
//     {
//         ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
//         await ctx.Response.WriteAsync("User ID is required.");
//         return;
//     }

//     if (!Guid.TryParse(userIdRouteValue, out Guid userIdGuid))
//     {
//         ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
//         await ctx.Response.WriteAsync("Invalid User ID format.");
//         return;
//     }

    // var userIdentityName = ctx.User.Identity.Name;
    // if (string.IsNullOrEmpty(userIdentityName))
    // {
    //     ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
    //     await ctx.Response.WriteAsync("Authentication is required.");
    //     return;
    // }

    // if (!Guid.TryParse(userIdentityName, out Guid userGuid))
    // {
    //     ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
    //     await ctx.Response.WriteAsync("User identity format is invalid. Identity must be a GUID.");
    //     return;
    // }

//     var command = new InviteFriend(userIdGuid, userIdGuid);
//     await ctx.RequestServices.GetRequiredService<ICommandDispatcher>().SendAsync(command);
//     ctx.Response.Created($"friends/{userIdRouteValue}/invite");
// })


    
                .UseLogging()
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
