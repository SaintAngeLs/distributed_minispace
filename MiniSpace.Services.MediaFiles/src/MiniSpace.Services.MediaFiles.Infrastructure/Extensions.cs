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
        public static IParalaxBuilder AddInfrastructure(this IParalaxBuilder builder)
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
                .UseParalax()
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
