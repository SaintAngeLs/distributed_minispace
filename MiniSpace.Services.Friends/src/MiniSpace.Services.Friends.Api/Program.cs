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
                        .Get<GetFriendRequests, IEnumerable<FriendRequestDto>>("friends/pending/all")
                        .Post<RemoveFriend>("friends/{friendId}/remove")
                        .Post<InviteFriend>("friends/{studentId}/invite", afterDispatch: (cmd, ctx) => ctx.Response.Created($"friends/{ctx.Request.RouteValues["userId"]}/invite")))) 
                .UseLogging()
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
