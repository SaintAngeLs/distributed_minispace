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
using MiniSpace.Services.Comments.Application.Dto;
using MiniSpace.Services.Comments.Application.Services;
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
                        .Post<SearchComments>("comments/search", async (cmd, ctx) =>
                        {
                            var pagedResult = await ctx.RequestServices.GetService<ICommentService>().BrowseCommentsAsync(cmd);
                            await ctx.Response.WriteJsonAsync(pagedResult);
                        }))
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Post<CreateComment>("comments")
                        .Post<SearchComments>("comments/search")
                        .Put<UpdateComment>("comments/{commentID}")
                        .Delete<DeleteComment>("comments/{commentID}")
                        .Post<AddLike>("comments/{commentID}/like")
                        .Delete<DeleteLike>("comments/{commentID}/like")
                    )
                )
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
