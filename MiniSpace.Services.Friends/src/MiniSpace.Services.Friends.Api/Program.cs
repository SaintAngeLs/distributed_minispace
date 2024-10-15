using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Paralax;
using Paralax.CQRS.Commands;
using Paralax.CQRS.Queries;
using Paralax.Logging;
using Paralax.Types;
using Paralax.WebApi;
using Paralax.CQRS.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Friends.Application;
using MiniSpace.Services.Friends.Core.Wrappers;
using MiniSpace.Services.Friends.Application.Commands;
using MiniSpace.Services.Friends.Application.Dto;
using MiniSpace.Services.Friends.Application.Queries;
using MiniSpace.Services.Friends.Infrastructure;
using Paralax.Types;
using Paralax.Core;


namespace MiniSpace.Services.Friends.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddParalax()
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
                        .Get<GetIncomingFriendRequests, IEnumerable<UserRequestsDto>>("friends/requests/{userId}")
                        .Get<GetSentFriendRequests, IEnumerable<UserRequestsDto>>("friends/requests/sent/{userId}")

                        .Get<GetIncomingFriendRequestsPaginated, PagedResponse<UserRequestsDto>>("friends/requests/{userId}/paginated")
                        .Get<GetSentFriendRequestsPaginated, PagedResponse<UserRequestsDto>>("friends/requests/sent/{userId}/paginated")


                        .Get<GetFriendRequests, PagedResponse<FriendRequestDto>>("friends/pending/all")
                        

                        .Get<GetFollowers, PagedResponse<UserFriendsDto>>("friends/{userId}/followers")
                        .Get<GetFollowing, PagedResponse<UserFriendsDto>>("friends/{userId}/following")

                        .Put<SentFriendRequestWithdraw>("friends/requests/{userId}/withdraw", afterDispatch: (cmd, ctx) => ctx.Response.Ok())
                        .Delete<RemoveFriend>("friends/{requesterId}/{friendId}/remove")
                       )) 
                .UseLogging()
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
