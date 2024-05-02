using System.Collections.Generic;
using System.Threading.Tasks;
using Convey;
using Convey.Secrets.Vault;
using Convey.Logging;
using Convey.Types;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Comments.Application;
using MiniSpace.Services.Comments.Application.Commands;
using MiniSpace.Services.Comments.Application.DTO;
using MiniSpace.Services.Comments.Application.Queries;
using MiniSpace.Services.Comments.Application.Services;
using MiniSpace.Services.Comments.Application.Wrappers;
using MiniSpace.Services.Comments.Infrastructure;

namespace MiniSpace.Services.Identity.Api
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
                    .UseEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                    .UseDispatcherEndpoints(endpoints => endpoints
                        //.Get<GetComment, IEnumerable<CommentDto>>("comments/{postId}")
                        .Post<CreateComment>("comments/{postId}",
                        //    afterDispatch: (cmd, ctx) => ctx.Response.Created($"commens/{cmd.PostId}")
                        )
                        .Put<UpdateComment>("comments/{postId}/{commentID}")
                        .Delete<DeleteComment>("comments/{postId}/{commentID}",
                        //    afterDispatch: (cmd, ctx) => ctx.Response.Created($"commens/{cmd.PostId}/{cmd.Id}")
                        //    to chyba w końcu nie potrzebne
                        )
                        // .Post<UpdateLike>("comments/{postId}/{commentID}/like",
                        // WIP
                        // afterDispatch: (cmd, ctx) => ctx.Response.Created($"comments/{cmd.postId}/{cmd.ID}/like")
                        // nie było w spec ale moze można dodać?
                        // )
                    )
                ))
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
