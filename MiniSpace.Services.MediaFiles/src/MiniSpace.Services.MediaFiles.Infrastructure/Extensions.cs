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
using MiniSpace.Services.MediaFiles.Application;
using MiniSpace.Services.MediaFiles.Application.Commands;

using MiniSpace.Services.MediaFiles.Application.Services;
using MiniSpace.Services.MediaFiles.Core.Repositories;
using MiniSpace.Services.MediaFiles.Infrastructure.Contexts;
using MiniSpace.Services.MediaFiles.Infrastructure.Decorators;
using MiniSpace.Services.MediaFiles.Infrastructure.Exceptions;
using MiniSpace.Services.MediaFiles.Infrastructure.Logging;
using MiniSpace.Services.MediaFiles.Infrastructure.Mongo.Documents;
using MiniSpace.Services.MediaFiles.Infrastructure.Mongo.Repositories;
using MiniSpace.Services.MediaFiles.Infrastructure.Services;
using MiniSpace.Services.MediaFiles.Infrastructure.Services.Workers;
using MongoDB.Driver;
using Amazon.S3;
using MiniSpace.Services.MediaFiles.Infrastructure.Options;

namespace MiniSpace.Services.MediaFiles.Infrastructure
{
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder.Services.AddTransient<IFileSourceInfoRepository, FileSourceInfoMongoRepository>();
            builder.Services.AddSingleton<IFileValidator, FileValidator>();
            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddSingleton<IEventMapper, EventMapper>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<IAppContextFactory, AppContextFactory>();
            builder.Services.AddTransient<IMediaFilesService, MediaFilesService>();
            builder.Services.AddTransient<IS3Service, S3Service>();
            builder.Services.AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>().Create());
            builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
            builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));
            builder.Services.AddSingleton<IGridFSService, GridFSService>(serviceProvider =>
            {
                var mongoDbOptions = serviceProvider.GetRequiredService<MongoDbOptions>();
                var mongoClient = new MongoClient(mongoDbOptions.ConnectionString);
                var database = mongoClient.GetDatabase(mongoDbOptions.Database);
                return new GridFSService(database);
            });
            
            var awsOptions = new AwsOptions
            {
                AccessKeyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"),
                SecretAccessKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"),
                Region = Environment.GetEnvironmentVariable("AWS_REGION")
            };
            builder.Services.AddSingleton(awsOptions);

            builder.Services.AddSingleton<IAmazonS3>(sp =>
            {
                var options = sp.GetRequiredService<AwsOptions>();
                return new AmazonS3Client(options.AccessKeyId, options.SecretAccessKey, Amazon.RegionEndpoint.GetBySystemName(options.Region));
            });

            // builder.Services.AddHostedService<FileCleanupWorker>();

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
                .AddMongoRepository<FileSourceInfoDocument, Guid>("fileSourceInfos")
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
                .SubscribeCommand<UploadMediaFile>()
                .SubscribeCommand<DeleteMediaFile>()
                .SubscribeCommand<CleanupUnassociatedFiles>();

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
