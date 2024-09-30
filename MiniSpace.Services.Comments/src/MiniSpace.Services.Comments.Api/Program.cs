using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Paralax;
using Paralax.Secrets.Vault;
using Paralax.Logging;
using Paralax.Types;
using Paralax.WebApi;
using Paralax.CQRS.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Comments.Application;
using MiniSpace.Services.Comments.Application.Commands;
using MiniSpace.Services.Comments.Application.Dto;
using MiniSpace.Services.Comments.Application.Queries;
using MiniSpace.Services.Comments.Application.Services;
using MiniSpace.Services.Comments.Infrastructure;
using MiniSpace.Services.Comments.Core.Wrappers;

namespace MiniSpace.Services.Identity.Api
{
    [ExcludeFromCodeCoverage]
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
                    .UseEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name)))
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get<GetComment, CommentDto>("comments/{commentID}")
                        .Get<SearchComments, PagedResponse<CommentDto>>("comments/search")
                        .Post<CreateComment>("comments")
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
