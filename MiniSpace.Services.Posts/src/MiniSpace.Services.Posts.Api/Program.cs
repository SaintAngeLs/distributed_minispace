using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Paralax;
using Paralax.Logging;
using Paralax.Core;
using Paralax.WebApi;
using Paralax.CQRS.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Posts.Application;
using MiniSpace.Services.Posts.Application.Commands;
using MiniSpace.Services.Posts.Application.Dto;
using MiniSpace.Services.Posts.Application.DTO;
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
                    .AddParalax()
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
                        .Get<GetUserFeed, PagedResponse<PostDto>>("posts/users/{userId}/feed")
                        .Get<GetOrganizerPosts, IEnumerable<PostDto>>("posts/organizer/{organizerId}")
                        .Post<ViewPost>("posts/{postId}/view",
                            afterDispatch: (cmd, ctx) => ctx.Response.NoContent())
                        .Get<GetUserPostViews, PagedResponse<ViewDto>>("posts/users/{userId}/views")
                        .Put<UpdatePost>("posts/{postId}")
                        .Delete<DeletePost>("posts/{postId}")
                        .Post<CreatePost>("posts",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"posts/{cmd.PostId}"))
                        .Put<ChangePostState>("posts/{postId}/state/{state}",
                            afterDispatch: (cmd, ctx) => ctx.Response.NoContent())
                        .Post<RepostCommand>("posts/{originalPostId}/repost", 
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"posts/{cmd.RepostedPostId}/reposted"))
                    ))
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
