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
using MiniSpace.Services.Posts.Application;
using MiniSpace.Services.Posts.Application.Commands;
using MiniSpace.Services.Posts.Application.Events.External;
using MiniSpace.Services.Posts.Application.Services;
using MiniSpace.Services.Posts.Core.Repositories;
using MiniSpace.Services.Posts.Infrastructure.Contexts;
using MiniSpace.Services.Posts.Infrastructure.Decorators;
using MiniSpace.Services.Posts.Infrastructure.Exceptions;
using MiniSpace.Services.Posts.Infrastructure.Logging;
using MiniSpace.Services.Posts.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Posts.Infrastructure.Mongo.Repositories;
using MiniSpace.Services.Posts.Infrastructure.Services;
using MiniSpace.Services.Posts.Infrastructure.Services.Workers;
using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Events.Infrastructure.Services.Clients;
using MiniSpace.Services.Posts.Application.Services.Clients;

namespace MiniSpace.Services.Posts.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder.Services.AddTransient<IPostRepository, PostMongoRepository>();
            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddSingleton<IEventMapper, EventMapper>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<IAppContextFactory, AppContextFactory>();
            builder.Services.AddTransient<IPostsService, PostsService>();

            builder.Services.AddTransient<IOrganizationEventPostRepository, OrganizationEventPostMongoRepository>();
            builder.Services.AddTransient<IOrganizationPostRepository, OrganizationPostMongoRepository>();
            builder.Services.AddTransient<IUserEventPostRepository, UserEventPostMongoRepository>();
            builder.Services.AddTransient<IUserPostRepository, UserPostMongoRepository>();
            builder.Services.AddTransient<IUserCommentsHistoryRepository, UserCommentsHistoryRepository>();
            builder.Services.AddTransient<IPostRecommendationService, PostRecommendationService>();

            builder.Services.AddTransient<IStudentsServiceClient, StudentsServiceClient>();
            builder.Services.AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>().Create());
            builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
            builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));
            builder.Services.AddHostedService<PostStateUpdaterWorker>();

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
                .AddMongoRepository<OrganizationEventPostDocument, Guid>("organization_events_posts")
                .AddMongoRepository<OrganizationPostDocument, Guid>("organization_posts")
                .AddMongoRepository<UserEventPostDocument, Guid>("user_events_posts")
                .AddMongoRepository<UserPostDocument, Guid>("user_posts")
                .AddMongoRepository<PostDocument, Guid>("posts")
                .AddMongoRepository<UserCommentsDocument, Guid>("user_comments_history")
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
                .SubscribeCommand<UpdatePost>()
                .SubscribeCommand<DeletePost>()
                .SubscribeCommand<CreatePost>()
                .SubscribeCommand<UpdatePostsState>()
                .SubscribeCommand<ChangePostState>()
                .SubscribeEvent<MediaFileDeleted>()
                .SubscribeEvent<CommentCreated>();

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
