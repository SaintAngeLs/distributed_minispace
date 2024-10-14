using System.Collections.Generic;
using System.Threading.Tasks;
using Paralax;
using Paralax.Logging;
using Paralax.Types;
using Paralax.WebApi;
using Paralax.CQRS.WebApi;
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
using Paralax.CQRS.Commands;
using System.Text;
using Paralax.Types;
using Paralax.Core;


namespace MiniSpace.Services.MediaFiles.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Env.Load();

            await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddParalax()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    // .UseMiddleware<RequestLoggingMiddleware>()
                    .UseInfrastructure()
                    .UseEndpoints(endpoints => endpoints
                        .Post<UploadMediaFile>("media-files", async (cmd, ctx) =>
                        {
                            var fileId = await ctx.RequestServices.GetService<IMediaFilesService>().UploadAsync(cmd);
                            await ctx.Response.WriteJsonAsync(fileId);
                        })
                        .Post<UploadFile>("files", async (cmd, ctx) =>
                        {
                            var fileId = await ctx.RequestServices.GetService<IMediaFilesService>().UploadFileAsync(cmd);
                            await ctx.Response.WriteJsonAsync(fileId);
                        })
                    )
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetMediaFile, FileDto>("media-files/{mediaFileId}")
                        .Get<GetOriginalMediaFile, FileDto>("media-files/{mediaFileId}/original")
                        .Delete<DeleteMediaFile>("media-files/delete/{mediaFileUrl}")
                    ))
                .UseLogging()
                .Build()
                .RunAsync();
        }
    }


    
    // public class RequestLoggingMiddleware
    // {
    //     private readonly RequestDelegate _next;

    //     public RequestLoggingMiddleware(RequestDelegate next)
    //     {
    //         _next = next;
    //     }

    //     public async Task Invoke(HttpContext context)
    //     {
    //         // Enable buffering so we can read the request body multiple times
    //         context.Request.EnableBuffering();

    //         // Read the request body as a string
    //         var requestBody = await new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true).ReadToEndAsync();
            
    //         // Log the request body
    //         Console.WriteLine("Request Body:");
    //         Console.WriteLine(requestBody);

    //         // Reset the request body stream position so the next middleware can read it
    //         context.Request.Body.Position = 0;

    //         // Continue processing the request
    //         await _next(context);
    //     }
    // }
}