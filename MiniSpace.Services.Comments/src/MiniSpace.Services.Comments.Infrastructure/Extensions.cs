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
using MiniSpace.Services.Comments.Application;
using MiniSpace.Services.Comments.Application.Commands;
using MiniSpace.Services.Comments.Application.Events.External;
using MiniSpace.Services.Comments.Application.Services;
using MiniSpace.Services.Comments.Core.Repositories;
using MiniSpace.Services.Comments.Infrastructure.Contexts;
using MiniSpace.Services.Comments.Infrastructure.Decorators;
using MiniSpace.Services.Comments.Infrastructure.Exceptions;
using MiniSpace.Services.Comments.Infrastructure.Logging;
using MiniSpace.Services.Comments.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Comments.Infrastructure.Mongo.Repositories;
using MiniSpace.Services.Comments.Infrastructure.Services;
using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Comments.Application.Services.Clients;
using MiniSpace.Services.Comments.Infrastructure.Services.Clients;

namespace MiniSpace.Services.Comments.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static IParalaxBuilder AddInfrastructure(this IParalaxBuilder builder)
        {
            builder.Services.AddTransient<ICommentRepository, CommentMongoRepository>();
            builder.Services.AddTransient<IOrganizationEventsCommentRepository, OrganizationEventsCommentRepository>();
            builder.Services.AddTransient<IOrganizationPostsCommentRepository, OrganizationPostsCommentRepository>();
            builder.Services.AddTransient<IUserEventsCommentRepository, UserEventsCommentRepository>();
            builder.Services.AddTransient<IUserPostsCommentRepository, UserPostsCommentRepository>();

            builder.Services.AddTransient<IStudentsServiceClient, StudentsServiceClient>();

            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddSingleton<IEventMapper, EventMapper>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
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
                .AddMongoRepository<CommentDocument, Guid>("comments")
                .AddMongoRepository<OrganizationEventCommentDocument, Guid>("organization_events_comments")
                .AddMongoRepository<OrganizationPostCommentDocument, Guid>("organization_posts_comments")
                .AddMongoRepository<UserEventCommentDocument, Guid>("user_events_comments")
                .AddMongoRepository<UserPostCommentDocument, Guid>("user_posts_comments")
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
                .SubscribeCommand<UpdateComment>()
                .SubscribeCommand<DeleteComment>()
                .SubscribeCommand<CreateComment>();

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
