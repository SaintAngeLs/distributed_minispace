using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
using MiniSpace.Services.Posts.Application;
using MiniSpace.Services.Posts.Application.Commands;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Application.Queries;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Wrappers;
using MiniSpace.Services.Posts.Infrastructure;

namespace MiniSpace.Services.Posts.Api
{
    [ExcludeFromCodeCoverage]
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
                        .Get<GetPost, PostDto>("posts/{postId}")
                        .Get<GetPosts, PagedResponse<PostDto>>("posts/search")
                        .Get<GetOrganizerPosts, IEnumerable<PostDto>>("posts/organizer/{organizerId}")
                        .Put<UpdatePost>("posts/{postId}")
                        .Delete<DeletePost>("posts/{postId}")
                        .Post<CreatePost>("posts",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"posts/{cmd.PostId}"))
                        .Put<ChangePostState>("posts/{postId}/state/{state}",
                            afterDispatch: (cmd, ctx) => ctx.Response.NoContent())
                    ))
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
