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
using MiniSpace.Services.Events.Application;
using MiniSpace.Services.Events.Application.Commands;
using MiniSpace.Services.Events.Application.DTO;
using MiniSpace.Services.Events.Application.Queries;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Application.Wrappers;
using MiniSpace.Services.Events.Infrastructure;
using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System.Text.Json;

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
                    // .UseMiddleware<RequestLoggingMiddleware>() 
                    // .UseMiddleware<ExceptionHandlingMiddleware>() 
                    .UseInfrastructure()
                    .UseEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Post<SearchEvents>("events/search", async (cmd, ctx) =>
                        {
                            var pagedResult = await ctx.RequestServices.GetService<IEventService>().BrowseEventsAsync(cmd);
                            await ctx.Response.WriteJsonAsync(pagedResult);
                        })
                        .Post<SearchOrganizerEvents>("events/search/organizer", async (cmd, ctx) =>
                        {
                            var pagedResult = await ctx.RequestServices.GetService<IEventService>().BrowseOrganizerEventsAsync(cmd);
                            await ctx.Response.WriteJsonAsync(pagedResult);
                        }))
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get<GetEvent, EventDto>("events/{eventId}")
                        .Put<UpdateEvent>("events/{eventId}")
                        .Post<CreateEvent>("events",
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"events/{cmd.EventId}"))
                        .Delete<DeleteEvent>("events/{eventId}")
                        .Post<SignUpToEvent>("events/{eventId}/sign-up")
                        .Delete<CancelSignUpToEvent>("events/{eventId}/sign-up")
                        .Post<ShowInterestInEvent>("events/{eventId}/show-interest")
                        .Delete<CancelInterestInEvent>("events/{eventId}/show-interest")
                        .Post<RateEvent>("events/{eventId}/rate")
                        .Delete<CancelRateEvent>("events/{eventId}/rate")
                        .Get<GetStudentEvents, PagedResponse<IEnumerable<EventDto>>>("events/student/{studentId}")
                        .Get<GetEventParticipants, EventParticipantsDto>("events/{eventId}/participants")
                        .Get<GetEventRating, EventRatingDto>("events/{eventId}/rating")
                        .Post<AddEventParticipant>("events/{eventId}/participants")
                        .Delete<RemoveEventParticipant>("events/{eventId}/participants")
                    )
                )
                .UseLogging()
                .Build()
                .RunAsync();
    }

    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Enable buffering so the stream can be read multiple times
            context.Request.EnableBuffering();

            // Read the stream as text
            var bodyAsText = await new StreamReader(context.Request.Body).ReadToEndAsync();

            // Log the request body
            Console.WriteLine("Received JSON:");
            Console.WriteLine(bodyAsText);

            // Reset the stream position to allow the next middleware to read it
            context.Request.Body.Position = 0;

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }

    public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = StatusCodes.Status500InternalServerError;
        var result = JsonSerializer.Serialize(new { error = exception.Message });

        if (exception is ArgumentException || exception is InvalidOperationException)
        {
            statusCode = StatusCodes.Status400BadRequest;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(result);
    }
}


}
