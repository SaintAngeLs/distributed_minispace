using System.Text;
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
using Paralax.WebApi.Security;
using Paralax.WebApi.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using MiniSpace.Services.Notifications.Application;
using MiniSpace.Services.Notifications.Application.Commands;
using MiniSpace.Services.Notifications.Application.Events.External;
using MiniSpace.Services.Notifications.Application.Events.External.Handlers;
using MiniSpace.Services.Notifications.Application.Services;
using MiniSpace.Services.Notifications.Core.Repositories;
using MiniSpace.Services.Notifications.Infrastructure.Contexts;
using MiniSpace.Services.Notifications.Infrastructure.Decorators;
using MiniSpace.Services.Notifications.Infrastructure.Exceptions;
using MiniSpace.Services.Notifications.Infrastructure.Logging;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Notifications.Infrastructure.Mongo.Repositories;
using MiniSpace.Services.Notifications.Infrastructure.Services;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Infrastructure.Services.Clients;
// using MiniSpace.Services.Notifications.Infrastructure.Managers;
using MiniSpace.Services.Notifications.Application.Hubs;
using MiniSpace.Services.Notifications.Application.Events.External.Comments;
using MiniSpace.Services.Notifications.Application.Events.External.Identity;
using MiniSpace.Services.Notifications.Application.Events.External.Reports;
using MiniSpace.Services.Notifications.Application.Events.External.Reactions;
using MiniSpace.Services.Notifications.Application.Events.External.Posts;
using MiniSpace.Services.Notifications.Application.Events.External.Friends;
using MiniSpace.Services.Notifications.Application.Events.External.Events;

namespace MiniSpace.Services.Notifications.Infrastructure
{
    public static class Extensions
    {
        public static IParalaxBuilder AddInfrastructure(this IParalaxBuilder builder)
        {
            builder.Services.AddSingleton<IEventMapper, EventMapper>();
            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            // builder.Services.AddSingleton<ISignalRConnectionManager, SignalRConnectionManager>();
            builder.Services.AddTransient<INotificationRepository, NotificationMongoRepository>();
            builder.Services.AddTransient<IFriendEventRepository, FriendEventMongoRepository>();
            builder.Services.AddTransient<IUserNotificationsRepository, UserNotificationsMongoRepository>();
            builder.Services.AddTransient<IExtendedUserNotificationsRepository, UserNotificationsMongoRepository>();
            builder.Services.AddSingleton<IBaseUrlService, BaseUrlService>();
            builder.Services.AddSingleton<IEventMapper, EventMapper>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<IAppContextFactory, AppContextFactory>();
            builder.Services.AddTransient<IFriendsServiceClient, FriendsServiceClient>();
            builder.Services.AddTransient<IStudentsServiceClient, StudentsServiceClient>();
            builder.Services.AddTransient<IEventsServiceClient, EventsServiceClient>();
            builder.Services.AddTransient<IPostsServiceClient, PostsServiceClient>();
            builder.Services.AddTransient<ICommentsServiceClient, CommentsServiceClient>();
            builder.Services.AddTransient<IReactionsServiceClient, ReactionsServiceClient>();
            builder.Services.AddTransient<IReportsServiceClient, ReportsServiceClient>();
            builder.Services.AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>().Create());
            builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
            builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));
            builder.Services.AddHostedService<DailyNotificationCleanupService>();  
            builder.Services.AddHostedService<PeriodicNotificationCleanupService>();  
            // builder.Services.AddScoped<INotificationHub, NotificationHub>();

            return builder
                .AddErrorHandler<ExceptionToResponseMapper>()
                .AddQueryHandlers()
                .AddInMemoryQueryDispatcher()
                .AddHttpClient()
                .AddConsul()
                .AddFabio()
                .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
                .AddMessageOutbox(o => o.AddMongo())
                .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
                .AddMongo()
                .AddRedis()
                .AddMetrics()
                .AddJaeger()
                .AddHandlersLogging()
                .AddMongoRepository<NotificationDocument, Guid>("notifications")
                .AddMongoRepository<FriendEventDocument, Guid>("friend-service")
                .AddMongoRepository<UserNotificationsDocument, Guid>("user_notifications")
                // .AddMongoRepository<FriendEventDocument, Guid>("events-service")
                .AddSignalRInfrastructure() 
                .AddWebApiSwaggerDocs()
                .AddCertificateAuthentication()
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
                .UseCertificateAuthentication()
                .UseRabbitMq()
                .SubscribeCommand<CreateNotification>()
                .SubscribeCommand<DeleteNotification>()
                .SubscribeCommand<UpdateNotificationStatus>()
                // .SubscribeEvent<FriendRequestCreated>()
                // .SubscribeEvent<FriendRequestCreated>()
                .SubscribeEvent<FriendRequestSent>()
                .SubscribeEvent<FriendInvited>()
                .SubscribeEvent<FriendAdded>()
                .SubscribeEvent<PendingFriendAccepted>()
                .SubscribeEvent<PendingFriendDeclined>()
                .SubscribeEvent<EventCreated>()
                .SubscribeEvent<EventDeleted>()
                .SubscribeEvent<StudentShowedInterestInEvent>()
                .SubscribeEvent<StudentCancelledInterestInEvent>()
                .SubscribeEvent<EventParticipantAdded>()
                .SubscribeEvent<EventParticipantRemoved>()
                .SubscribeEvent<StudentSignedUpToEvent>()
                .SubscribeEvent<StudentCancelledSignUpToEvent>()
                .SubscribeEvent<PostCreated>()
                .SubscribeEvent<PostUpdated>()
                .SubscribeEvent<PasswordResetTokenGenerated>()
                .SubscribeEvent<SignedUp>()
                .SubscribeEvent<CommentCreated>()
                .SubscribeEvent<CommentUpdated>()
                .SubscribeEvent<ReactionCreated>()
                .SubscribeEvent<ReportCreated>()
                .SubscribeEvent<ReportReviewStarted>()
                .SubscribeEvent<ReportResolved>()
                .SubscribeEvent<ReportRejected>()
                .SubscribeEvent<ReportCancelled>()
                .SubscribeEvent<EmailVerified>()
                .SubscribeEvent<TwoFactorCodeGenerated>();
            return app;
        }

        public static IParalaxBuilder AddSignalRInfrastructure(this IParalaxBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowed((host) => true)); 
            });

            builder.Services.AddSignalR();

            return builder;
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
