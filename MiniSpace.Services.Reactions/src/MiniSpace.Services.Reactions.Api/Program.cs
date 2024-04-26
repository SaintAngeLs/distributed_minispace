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
                        .Post<CreateReaction>("reactions"
                                //, afterDispatch: (cmd, ctx) => ctx.Response.Created($"reactions/{cmd.ReactionId}")
                                )
                        .Delete<DeleteReaction>("reactions/{reactionId}")
                        //.Get<GetPosts, IEnumerable<PostDto>>("posts")
                        //.Put<UpdatePost>("posts/{postId}")
                        //.Delete<DeletePost>("posts/{postId}")
                        //.Post<CreatePost>("posts", afterDispatch: (cmd, ctx) => ctx.Response.Created($"posts/{cmd.PostId}"))
                        //.Put<ChangePostState>("posts/{postId}/state/{state}", afterDispatch: (cmd, ctx) => ctx.Response.NoContent())
                    ))
                .UseLogging()
                .Build()
                .RunAsync();
    }
}