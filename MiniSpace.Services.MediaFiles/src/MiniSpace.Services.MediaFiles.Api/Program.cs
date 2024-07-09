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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.MediaFiles.Application;
using MiniSpace.Services.MediaFiles.Application.Commands;
using MiniSpace.Services.MediaFiles.Application.Dto;
using MiniSpace.Services.MediaFiles.Application.Queries;
using MiniSpace.Services.MediaFiles.Application.Services;
using MiniSpace.Services.MediaFiles.Infrastructure;
using DotNetEnv;

namespace MiniSpace.Services.MediaFiles.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Env.Load();

            await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddConvey()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    .UseInfrastructure()
                    .UseEndpoints(endpoints => endpoints
                        .Post<UploadMediaFile>("media-files", async (cmd, ctx) =>
                        {
                            var fileId = await ctx.RequestServices.GetService<IMediaFilesService>().UploadAsync(cmd);
                            await ctx.Response.WriteJsonAsync(fileId);
                        })
                    )
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetMediaFile, FileDto>("media-files/{mediaFileId}")
                        .Get<GetOriginalMediaFile, FileDto>("media-files/{mediaFileId}/original")
                        .Delete<DeleteMediaFile>("media-files/{mediaFileId}")
                    ))
                .UseLogging()
                .Build()
                .RunAsync();
        }
    }
}