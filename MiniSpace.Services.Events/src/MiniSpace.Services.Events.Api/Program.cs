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
                        .Post<SearchEvents>("events/search", async (cmd, ctx) =>
                        {
                            var pagedResult = await ctx.RequestServices.GetService<IEventService>().SignInAsync(cmd);
                            await ctx.Response.WriteJsonAsync(pagedResult);
                        }))
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get<GetEvent, EventDto>("events/{eventId}")
                        //.Get<GetEventsOrganizer, IEnumerable<EventDto>>("events/organizer/{organizerId}")
                        .Put<UpdateEvent>("events/{eventId}")
                        .Post<AddEvent>("events", 
                            afterDispatch: (cmd, ctx) => ctx.Response.Created($"events/{cmd.EventId}"))
                        .Post<SignUpToEvent>("events/{eventId}/sign-up")
                        .Delete<CancelSignUpToEvent>("events/{eventId}/sign-up")
                        .Post<ShowInterestInEvent>("events/{eventId}/show-interest")
                        .Delete<CancelInterestInEvent>("events/{eventId}/show-interest")
                        .Post<RateEvent>("events/{eventId}/rate")
                        .Get<GetStudentEvents, PagedResponse<IEnumerable<EventDto>>>("events/student/{studentId}")
                        .Delete<DeleteEvent>("events/{eventId}")
                    )
                )
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
