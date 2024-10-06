using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paralax;
using Paralax.CQRS.Commands;
using Paralax.CQRS.Events;
using Paralax.CQRS.Queries;
using Paralax.Discovery.Consul;
using Paralax.Docs.Swagger;
using Paralax.HTTP;
using Paralax.LoadBalancing.Fabio;
using Paralax.MessageBrokers;
using Paralax.MessageBrokers.CQRS;
using Paralax.MessageBrokers.Outbox;
using Paralax.MessageBrokers.Outbox.Mongo;
using Paralax.MessageBrokers.RabbitMQ;
using Paralax.Metrics.AppMetrics;
using Paralax.Persistence.MongoDB;
using Paralax.Persistence.Redis;
using Paralax.Security;
using Paralax.Tracing.Jaeger;
using Paralax.Tracing.Jaeger.RabbitMQ;
using Paralax.WebApi;
using Paralax.CQRS.WebApi;
using Paralax.WebApi.Swagger;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using MiniSpace.Services.Events.Application;
using MiniSpace.Services.Events.Application.Commands;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Events.External;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Application.Services.Clients;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Infrastructure.Contexts;
using MiniSpace.Services.Events.Infrastructure.Decorators;
using MiniSpace.Services.Events.Infrastructure.Exceptions;
using MiniSpace.Services.Events.Infrastructure.Logging;
using MiniSpace.Services.Events.Infrastructure.Mongo;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Events.Infrastructure.Mongo.Repositories;
using MiniSpace.Services.Events.Infrastructure.Services;
using MiniSpace.Services.Events.Infrastructure.Services.Clients;
using MiniSpace.Services.Events.Infrastructure.Services.Workers;
using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Posts.Infrastructure.Mongo.Repositories;
using MiniSpace.Services.Events.Infrastructure.Services.Recommendation;
using Microsoft.ML;

namespace MiniSpace.Services.Events.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static IParalaxBuilder AddInfrastructure(this IParalaxBuilder builder)
        {
            builder.Services.AddSingleton<IEventMapper, EventMapper>();
            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddSingleton<IEventValidator, EventValidator>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            
            builder.Services.AddTransient<IEventRepository, EventMongoRepository>();
            builder.Services.AddTransient<IEventsUserViewsRepository, EventsUserViewsRepository>();
            builder.Services.AddTransient<IUserCommentsHistoryRepository, UserCommentsHistoryMongoRepository>();
            builder.Services.AddTransient<IUserReactionsHistoryRepository, UserReactionsHistoryMongoRepository>();

            builder.Services.AddTransient<IAppContextFactory, AppContextFactory>();
            builder.Services.AddTransient<IEventService, EventService>();
            builder.Services.AddTransient<IEventRecommendationService, EventRecommendationService>();
            builder.Services.AddTransient<IStudentsServiceClient, StudentsServiceClient>();
            builder.Services.AddTransient<IFriendsServiceClient, FriendsServiceClient>();
            builder.Services.AddTransient<IOrganizationsServiceClient, OrganizationsServiceClient>();
            builder.Services.AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>().Create());
            builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
            builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));
            builder.Services.AddSingleton<MLContext>();
            builder.Services.AddHostedService<EventStateUpdaterWorker>();

            return builder
                .AddErrorHandler<ExceptionToResponseMapper>()
                .AddQueryHandlers()
                .AddInMemoryQueryDispatcher()
                .AddHttpClient()
                .AddConsul()
                .AddFabio()
                .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
                .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
                .AddMessageOutbox(o => o.AddMongo())
                .AddMongo()
                .AddRedis()
                .AddMetrics()
                .AddJaeger()
                .AddHandlersLogging()
                .AddMongoRepository<EventDocument, Guid>("events")
                .AddMongoRepository<UserEventsViewsDocument, Guid>("events_views")
                .AddMongoRepository<UserCommentsDocument, Guid>("user_comments_history")
                .AddMongoRepository<UserReactionDocument, Guid>("events")
                .AddWebApiSwaggerDocs()
                .AddSecurity();
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseErrorHandler()
                .UseSwaggerDocs()
                .UseJaeger()
                .UseParalax()
                .UsePublicContracts<ContractAttribute>()
                .UseMetrics()
                .UseAuthentication()
                .UseRabbitMq()
                .SubscribeCommand<CreateEvent>()
                .SubscribeCommand<DeleteEvent>()
                .SubscribeCommand<UpdateEvent>()
                .SubscribeCommand<RateEvent>()
                .SubscribeCommand<SignUpToEvent>()
                .SubscribeCommand<ShowInterestInEvent>()
                .SubscribeCommand<CancelInterestInEvent>()
                .SubscribeCommand<CancelSignUpToEvent>()
                .SubscribeCommand<AddEventParticipant>()
                .SubscribeCommand<RemoveEventParticipant>()
                .SubscribeEvent<MediaFileDeleted>()
                .SubscribeEvent<EventImageUploaded>()
                .SubscribeEvent<CommentCreated>()
                .SubscribeEvent<ReactionCreated>();

            return app;
        }

        internal static CorrelationContext GetCorrelationContext(this IHttpContextAccessor accessor)
            => accessor.HttpContext?.Request.Headers.TryGetValue("Correlation-Context", out var json) is true
                ? JsonConvert.DeserializeObject<CorrelationContext>(json.FirstOrDefault())
                : null;
        
        internal static IDictionary<string, object> GetHeadersToForward(this IMessageProperties messageProperties)
        {
            const string sagaHeader = "Saga";
            if (messageProperties?.Headers is null || !messageProperties.Headers.TryGetValue(sagaHeader, out var saga))
            {
                return null;
            }

            return saga is null
                ? null
                : new Dictionary<string, object>
                {
                    [sagaHeader] = saga
                };
        }
        
        internal static string GetSpanContext(this IMessageProperties messageProperties, string header)
        {
            if (messageProperties is null)
            {
                return string.Empty;
            }

            if (messageProperties.Headers.TryGetValue(header, out var span) && span is byte[] spanBytes)
            {
                return Encoding.UTF8.GetString(spanBytes);
            }

            return string.Empty;
        }
    }
}