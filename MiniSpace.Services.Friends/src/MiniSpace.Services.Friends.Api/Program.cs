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
// using MiniSpace.Services.Friends.Application.Events;
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
                        // .Get<IEnumerable<FriendDto>>("friends/{studentId}", 
                        //     ctx => new GetFriends { StudentId = Guid.Parse(ctx.Request.RouteValues["studentId"].ToString()) }, 
                        //     (query, ctx) => ctx.Response.WriteAsJsonAsync(query), // Correctly define delegate with parameters
                        //     afterDispatch: ctx => ctx.Response.Ok())
                        .Get<GetFriends, IEnumerable<StudentFriendsDto>>("friends/{studentId}") 
                        .Get<GetIncomingFriendRequests, IEnumerable<StudentRequestsDto>>("friends/requests/{studentId}")
                        // .Get<GetFriends, IEnumerable<FriendDto>>("friends/pending")
                        .Get<GetFriendRequests, IEnumerable<FriendRequestDto>>("friends/pending/all")
                        .Get<GetSentFriendRequests, IEnumerable<StudentRequestsDto>>("friends/requests/sent/{studentId}")
                        // .Get("friends/requests/sent", ctx =>
                        // {
                        //     var query = new GetSentFriendRequests { StudentId = ctx.User.GetUserId() }; 
                        //     return ctx.QueryDispatcher.QueryAsync(query);
                        // }, afterDispatch: ctx => ctx.Response.WriteAsJsonAsync(ctx.Result))

                        .Post<PendingFriendAccept>("friends/requests/{studentId}/accept", afterDispatch: (cmd, ctx) => ctx.Response.Ok())
                        .Post<PendingFriendDecline>("friends/requests/{studentId}/decline", afterDispatch: (cmd, ctx) => ctx.Response.Ok())
                        .Put<SentFriendRequestWithdraw>("friends/requests/{studentId}/withdraw", afterDispatch: (cmd, ctx) => ctx.Response.Ok())
                        .Delete<RemoveFriend>("friends/{requesterId}/{friendId}/remove")
                        .Post<InviteFriend>("friends/{studentId}/invite", afterDispatch: (cmd, ctx) => ctx.Response.Created($"friends/{ctx.Request.RouteValues["studentId"]}/invite")))) 
                .UseLogging()
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
