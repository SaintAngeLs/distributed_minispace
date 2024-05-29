using System.Text;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.Discovery.Consul;
using Convey.Docs.Swagger;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.MessageBrokers;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.Outbox.Mongo;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.AppMetrics;
using Convey.Persistence.MongoDB;
using Convey.Persistence.Redis;
using Convey.Security;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Convey.WebApi.Security;
using Convey.WebApi.Swagger;
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
using MiniSpace.Services.Notifications.Infrastructure;
using MiniSpace.Services.Notifications.Application.Services.Clients;
using MiniSpace.Services.Notifications.Infrastructure.Services.Clients;

namespace MiniSpace.Services.Notifications.Infrastructure
{
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder.Services.AddSingleton<IEventMapper, EventMapper>();
            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddTransient<INotificationRepository, NotificationMongoRepository>();
            builder.Services.AddTransient<IFriendEventRepository, FriendEventMongoRepository>();
            builder.Services.AddTransient<IStudentNotificationsRepository, StudentNotificationsMongoRepository>();
            builder.Services.AddTransient<IStudentRepository, StudentMongoRepository>();
            builder.Services.AddTransient<IExtendedStudentNotificationsRepository, StudentNotificationsMongoRepository>();
            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddSingleton<IEventMapper, EventMapper>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<IAppContextFactory, AppContextFactory>();
            builder.Services.AddTransient<IFriendsServiceClient, FriendsServiceClient>();
            builder.Services.AddTransient<IStudentsServiceClient, StudentsServiceClient>();
            builder.Services.AddTransient<IEventsServiceClient, EventsServiceClient>();
            builder.Services.AddTransient<IPostsServiceClient, PostsServiceClient>();
            builder.Services.AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>().Create());
            builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
            builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));
            builder.Services.AddHostedService<NotificationCleanupService>();  

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
                .AddMongoRepository<StudentDocument, Guid>("students")
                .AddMongoRepository<StudentNotificationsDocument, Guid>("students-notifications")
                // .AddMongoRepository<FriendEventDocument, Guid>("events-service")
                .AddWebApiSwaggerDocs()
                .AddCertificateAuthentication()
                .AddSecurity();
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseErrorHandler()
                .UseSwaggerDocs()
                .UseJaeger()
                .UseConvey()
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
                // .SubscribeEvent<NotificationCreated>()
                .SubscribeEvent<StudentSignedUpToEvent>()
                .SubscribeEvent<StudentCancelledSignUpToEvent>();
                // .SubscribeEvent<NotificationCreated>()
                // .SubscribeEvent<NotificationDeleted>()
                // .SubscribeEvent<NotificationUpdated>();

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
