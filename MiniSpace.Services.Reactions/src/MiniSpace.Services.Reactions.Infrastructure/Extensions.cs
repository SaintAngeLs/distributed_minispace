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
using MiniSpace.Services.Reactions.Application;
using MiniSpace.Services.Reactions.Application.Commands;
using MiniSpace.Services.Reactions.Application.Services;
using MiniSpace.Services.Reactions.Core.Repositories;
using MiniSpace.Services.Reactions.Infrastructure.Contexts;
using MiniSpace.Services.Reactions.Infrastructure.Decorators;
using MiniSpace.Services.Reactions.Infrastructure.Exceptions;
using MiniSpace.Services.Reactions.Infrastructure.Logging;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Reactions.Infrastructure.Mongo.Repositories;
using MiniSpace.Services.Reactions.Infrastructure.Services;
using MiniSpace.Services.Reactions.Application.Queries;
using Convey.Logging.CQRS;
using MiniSpace.Services.Reactions.Application.Events;
using MiniSpace.Services.Reactions.Application.Events.External;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Reactions.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            // add repositories
            builder.Services.AddTransient<IReactionRepository, ReactionMongoRepository>();
            builder.Services.AddTransient<IPostRepository, PostMongoRepository>();
            builder.Services.AddTransient<IEventRepository, EventMongoRepository>();
            builder.Services.AddTransient<IStudentRepository, StudentMongoRepository>();

            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddSingleton<IEventMapper, EventMapper>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<IAppContextFactory, AppContextFactory>();
            builder.Services.AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>().Create());
            builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
            builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));

            // background workers: none

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
                .AddMongoRepository<ReactionDocument, Guid>("reactions")
                .AddMongoRepository<PostDocument, Guid>("posts")
                .AddMongoRepository<EventDocument, Guid>("events")
                .AddMongoRepository<StudentDocument, Guid>("students")
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
                .SubscribeCommand<CreateReaction>()
                .SubscribeCommand<DeleteReaction>()
                .SubscribeEvent<EventCreated>()
                .SubscribeEvent<PostCreated>()
                .SubscribeEvent<StudentCreated>();

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
