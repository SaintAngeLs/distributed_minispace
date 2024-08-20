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

using MiniSpace.Services.Reactions.Application;
using MiniSpace.Services.Reactions.Application.Commands;
using MiniSpace.Services.Reactions.Application.Dto;
using MiniSpace.Services.Reactions.Application.Queries;
using MiniSpace.Services.Reactions.Core.Entities;
using MiniSpace.Services.Reactions.Infrastructure;

namespace MiniSpace.Services.Reactions.Api
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
                        .Get<GetReactions, IEnumerable<ReactionDto>>("reactions")
                        .Get<GetReactionsSummary, ReactionsSummaryDto>("reactions/summary")
                        .Post<CreateReaction>("reactions",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"reactions/{cmd.ReactionId}"))
                        .Put<UpdateReaction>("reactions/{reactionId}",  
                            afterDispatch: (cmd, ctx) => ctx.Response.NoContent())
                        .Delete<DeleteReaction>("reactions/{reactionId}")
                    ))
                .UseLogging()
                .Build()
                .RunAsync();
    }
}