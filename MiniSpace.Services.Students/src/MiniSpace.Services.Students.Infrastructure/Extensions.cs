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
using MiniSpace.Services.Students.Application;
using MiniSpace.Services.Students.Application.Commands;
using MiniSpace.Services.Students.Application.Events.External;
using MiniSpace.Services.Students.Application.Events.External.Handlers;
using MiniSpace.Services.Students.Application.Services;
using MiniSpace.Services.Students.Core.Repositories;
using MiniSpace.Services.Students.Infrastructure.Contexts;
using MiniSpace.Services.Students.Infrastructure.Decorators;
using MiniSpace.Services.Students.Infrastructure.Exceptions;
using MiniSpace.Services.Students.Infrastructure.Logging;
using MiniSpace.Services.Students.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Students.Infrastructure.Mongo.Repositories;
using MiniSpace.Services.Students.Infrastructure.Services;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Paralax.Types;
using Paralax.CQRS.WebApi;

namespace MiniSpace.Services.Students.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static IParalaxBuilder AddInfrastructure(this IParalaxBuilder builder)
        {
            builder.Services.AddTransient<IStudentRepository, StudentMongoRepository>();
            builder.Services.AddTransient<IUserNotificationPreferencesRepository, UserNotificationPreferencesRepository>();
            builder.Services.AddTransient<IUserSettingsRepository, UserSettingsRepository>();
            builder.Services.AddTransient<IUserGalleryRepository, UserGalleryRepository>();
            builder.Services.AddTransient<IUserProfileViewsForUserRepository, UserProfileViewsRepository>();
            builder.Services.AddTransient<IUserViewingProfilesRepository, UserViewingProfilesRepository>();
            builder.Services.AddTransient<IBlockedUsersRepository, BlockedUsersMongoRepository>();

            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddSingleton<IEventMapper, EventMapper>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<IDeviceInfoService, DeviceInfoService>();
            builder.Services.AddTransient<IAppContextFactory, AppContextFactory>();
            builder.Services.AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>().Create());
            builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
            builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));

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
                .AddMongoRepository<StudentDocument, Guid>("students")
                .AddMongoRepository<UserNotificationsDocument, Guid>("user-notifications")
                .AddMongoRepository<UserSettingsDocument, Guid>("user-settings")
                .AddMongoRepository<UserGalleryDocument, Guid>("user-gellery")
                .AddMongoRepository<UserProfileViewsDocument, Guid>("user_profile_views")
                .AddMongoRepository<UserViewingProfilesDocument, Guid>("user_viewing_profiles")
                .AddMongoRepository<BlockedUsersDocument, Guid>("blocked_users")
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
                .SubscribeCommand<UpdateStudent>()
                .SubscribeCommand<DeleteStudent>()
                .SubscribeCommand<CompleteStudentRegistration>()
                .SubscribeCommand<ChangeStudentState>()
                .SubscribeCommand<UpdateUserSettings>()
                .SubscribeCommand<UpdateStudentLanguagesAndInterests>()
                .SubscribeEvent<SignedUp>()
                .SubscribeEvent<EmailVerified>()
                .SubscribeEvent<SignedIn>()
                .SubscribeEvent<SignedOut>()
                .SubscribeEvent<TokenRefreshed>()
                .SubscribeEvent<UserStatusChanged>()
                .SubscribeEvent<StudentShowedInterestInEvent>()
                .SubscribeEvent<StudentCancelledInterestInEvent>()
                .SubscribeEvent<StudentSignedUpToEvent>()
                .SubscribeEvent<StudentCancelledSignUpToEvent>()
                .SubscribeEvent<UserBanned>()
                .SubscribeEvent<EventArchived>()
                .SubscribeEvent<UserUnbanned>()
                .SubscribeEvent<StudentImageUploaded>()
                .SubscribeEvent<MediaFileDeleted>()
                .SubscribeEvent<TwoFactorAuthenticationEnabled>()
                .SubscribeEvent<TwoFactorAuthenticationDisabled>();

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
