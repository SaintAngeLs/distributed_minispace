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
using Paralax.Security.WebApi;
using Paralax.WebApi.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using MiniSpace.Services.Email.Application;
using MiniSpace.Services.Email.Application.Commands;
using MiniSpace.Services.Email.Application.Events.External;
using MiniSpace.Services.Email.Application.Events.External.Handlers;
using MiniSpace.Services.Email.Application.Services;
using MiniSpace.Services.Email.Core.Repositories;
using MiniSpace.Services.Email.Infrastructure.Contexts;
using MiniSpace.Services.Email.Infrastructure.Decorators;
using MiniSpace.Services.Email.Infrastructure.Exceptions;
using MiniSpace.Services.Email.Infrastructure.Logging;
using MiniSpace.Services.Email.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Email.Infrastructure.Mongo.Repositories;
using MiniSpace.Services.Email.Infrastructure.Services;
using MiniSpace.Services.Email.Infrastructure;
using MiniSpace.Services.Email.Application.Services.Clients;
using MiniSpace.Services.Email.Infrastructure.Services.Clients;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;


namespace MiniSpace.Services.Email.Infrastructure
{
    public static class Extensions
    {
        public static IParalaxBuilder AddInfrastructure(this IParalaxBuilder builder)
        {
            var smtpConfig = builder.Services.BuildServiceProvider().GetService<IConfiguration>().GetSection("smtp").Get<SmtpSettings>();
            builder.Services.AddSingleton(smtpConfig);

            builder.Services.AddSingleton<IEmailService, SmtpEmailService>(provider =>
            new SmtpEmailService(
                smtpConfig.Host,
                smtpConfig.Port,
                smtpConfig.FromEmail,
                smtpConfig.Password,
                smtpConfig.EnableSSL,
                smtpConfig.DisplaySenderEmail 
            ));

            builder.Services.AddSingleton<IEventMapper, EventMapper>();
            builder.Services.AddTransient<IStudentEmailsRepository, StudentEmailsMongoRepository>();
            builder.Services.AddSingleton<IEventMapper, EventMapper>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<IAppContextFactory, AppContextFactory>();
            builder.Services.AddSingleton<Application.Services.IDateTimeProvider, Services.DateTimeProvider>();
            builder.Services.AddTransient<IStudentsServiceClient, StudentsServiceClient>();
            builder.Services.AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>().Create());
            builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
            builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));
            // builder.Services.AddHostedService<NotificationCleanupService>();  

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
                .AddMongoRepository<StudentEmailsDocument, Guid>("student-emails")
                .AddMongoRepository<StudentDocument, Guid>("students")
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
                .UseParalax()
                .UsePublicContracts<ContractAttribute>()
                .UseMetrics()
                .UseCertificateAuthentication()
                .UseRabbitMq()
                .SubscribeCommand<CreateEmailNotification>()
                // .SubscribeEvent<FriendRequestCreated>()
                // .SubscribeEvent<FriendRequestCreated>()
                .SubscribeEvent<NotificationCreated>();
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
