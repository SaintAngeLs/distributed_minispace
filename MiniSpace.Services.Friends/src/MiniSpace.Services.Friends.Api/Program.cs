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
using MiniSpace.Services.Friends.Core.Wrappers;
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
                        .Post<PendingFriendAccept>("friends/requests/{userId}/accept", afterDispatch: (cmd, ctx) => ctx.Response.Ok())
                        .Post<PendingFriendDecline>("friends/requests/{userId}/decline", afterDispatch: (cmd, ctx) => ctx.Response.Ok())
                        .Post<InviteFriend>("friends/{userId}/invite", afterDispatch: (cmd, ctx) => ctx.Response.Created($"friends/{ctx.Request.RouteValues["userId"]}/invite"))

                        .Get<GetFriends, PagedResponse<UserFriendsDto>>("friends/{userId}")
                        .Get<GetIncomingFriendRequests, PagedResponse<UserRequestsDto>>("friends/requests/{userId}")
                        .Get<GetFriendRequests, PagedResponse<FriendRequestDto>>("friends/pending/all")
                        .Get<GetSentFriendRequests, PagedResponse<UserRequestsDto>>("friends/requests/sent/{userId}")

                        .Get<GetFollowers, PagedResponse<UserFriendsDto>>("friends/{userId}/followers")
                        .Get<GetFollowing, PagedResponse<UserFriendsDto>>("friends/{userId}/following")

                        .Put<SentFriendRequestWithdraw>("friends/requests/{userId}/withdraw", afterDispatch: (cmd, ctx) => ctx.Response.Ok())

                        
                        // .Get<IEnumerable<FriendDto>>("friends/{studentId}", 
                        //     ctx => new GetFriends { StudentId = Guid.Parse(ctx.Request.RouteValues["studentId"].ToString()) }, 
                        //     (query, ctx) => ctx.Response.WriteAsJsonAsync(query), // Correctly define delegate with parameters
                        //     afterDispatch: ctx => ctx.Response.Ok())
                       
                        // .Get("friends/requests/sent", ctx =>
                        // {
                        //     var query = new GetSentFriendRequests { StudentId = ctx.User.GetUserId() }; 
                        //     return ctx.QueryDispatcher.QueryAsync(query);
                        // }, afterDispatch: ctx => ctx.Response.WriteAsJsonAsync(ctx.Result))
                        .Delete<RemoveFriend>("friends/{requesterId}/{friendId}/remove")
                       )) 
                .UseLogging()
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
