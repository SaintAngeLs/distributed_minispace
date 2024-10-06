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
using MiniSpace.Services.Communication.Application;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Application.Services;
using MiniSpace.Services.Communication.Core.Repositories;
using MiniSpace.Services.Communication.Infrastructure.Contexts;
using MiniSpace.Services.Communication.Infrastructure.Decorators;
using MiniSpace.Services.Communication.Infrastructure.Exceptions;
using MiniSpace.Services.Communication.Infrastructure.Logging;
using MiniSpace.Services.Communication.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Communication.Infrastructure.Mongo.Repositories;
using MiniSpace.Services.Communication.Infrastructure.Services;
using MiniSpace.Services.Communication.Application.Services.Clients;
using MiniSpace.Services.Communication.Infrastructure.Services.Clients;
using MiniSpace.Services.Communication.Application.Hubs;

namespace MiniSpace.Services.Communication.Infrastructure
{
    public static class Extensions
    {
        public static IParalaxBuilder AddInfrastructure(this IParalaxBuilder builder)
        {
            builder.Services.AddSingleton<IEventMapper, EventMapper>();
            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            builder.Services.AddTransient<IOrganizationChatsRepository, OrganizationChatsRepository>();
            builder.Services.AddTransient<IUserChatsRepository, UserChatsRepository>();
            builder.Services.AddTransient<ICommunicationService, CommunicationService>();
            
            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<IAppContextFactory, AppContextFactory>();
            builder.Services.AddTransient<IStudentsServiceClient, StudentsServiceClient>();

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
                .AddMongoRepository<OrganizationChatsDocument, Guid>("organizations_chats")
                .AddMongoRepository<UserChatsDocument, Guid>("user_chats")
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
                .SubscribeCommand<AddUserToChat>()
                .SubscribeCommand<CreateChat>()
                .SubscribeCommand<DeleteChat>()
                .SubscribeCommand<DeleteMessage>()
                .SubscribeCommand<RemoveUserFromChat>()
                .SubscribeCommand<SendMessage>()
                .SubscribeCommand<UpdateMessageStatus>()
                ;
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
