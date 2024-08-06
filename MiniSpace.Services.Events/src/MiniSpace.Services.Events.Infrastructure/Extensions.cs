﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Convey.WebApi.Swagger;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using MiniSpace.Services.Events.Application;
using MiniSpace.Services.Events.Application.Commands;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Events.External;
using MiniSpace.Services.Events.Application.Services;
using MiniSpace.Services.Events.Application.Services.Clients;
using MiniSpace.Services.Events.Core.Repositories;
using MiniSpace.Services.Events.Infrastructure.Contexts;
using MiniSpace.Services.Events.Infrastructure.Decorators;
using MiniSpace.Services.Events.Infrastructure.Exceptions;
using MiniSpace.Services.Events.Infrastructure.Logging;
using MiniSpace.Services.Events.Infrastructure.Mongo;
using MiniSpace.Services.Events.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Events.Infrastructure.Mongo.Repositories;
using MiniSpace.Services.Events.Infrastructure.Services;
using MiniSpace.Services.Events.Infrastructure.Services.Clients;
using MiniSpace.Services.Events.Infrastructure.Services.Workers;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder.Services.AddSingleton<IEventMapper, EventMapper>();
            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddSingleton<IEventValidator, EventValidator>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<IEventRepository, EventMongoRepository>();
            builder.Services.AddTransient<IAppContextFactory, AppContextFactory>();
            builder.Services.AddTransient<IEventService, EventService>();
            builder.Services.AddTransient<IStudentsServiceClient, StudentsServiceClient>();
            builder.Services.AddTransient<IFriendsServiceClient, FriendsServiceClient>();
            builder.Services.AddTransient<IOrganizationsServiceClient, OrganizationsServiceClient>();
            builder.Services.AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>().Create());
            builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
            builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));
            builder.Services.AddHostedService<EventStateUpdaterWorker>();

            return builder
                .AddErrorHandler<ExceptionToResponseMapper>()
                .AddQueryHandlers()
                .AddInMemoryQueryDispatcher()
                .AddHttpClient()
                .AddConsul()
                .AddFabio()
                .AddExceptionToMessageMapper<ExceptionToMessageMapper>()
                .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
                .AddMessageOutbox(o => o.AddMongo())
                .AddMongo()
                .AddRedis()
                .AddMetrics()
                .AddJaeger()
                .AddHandlersLogging()
                .AddMongoRepository<EventDocument, Guid>("events")
                .AddWebApiSwaggerDocs()
                .AddSecurity();
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseErrorHandler()
                .UseSwaggerDocs()
                .UseJaeger()
                .UseConvey()
                //.UseMongo()
                .UsePublicContracts<ContractAttribute>()
                .UseMetrics()
                .UseAuthentication()
                .UseRabbitMq()
                .SubscribeCommand<CreateEvent>()
                .SubscribeCommand<DeleteEvent>()
                .SubscribeCommand<UpdateEvent>()
                .SubscribeCommand<RateEvent>()
                .SubscribeCommand<SignUpToEvent>()
                .SubscribeCommand<ShowInterestInEvent>()
                .SubscribeCommand<CancelInterestInEvent>()
                .SubscribeCommand<CancelSignUpToEvent>()
                .SubscribeCommand<AddEventParticipant>()
                .SubscribeCommand<RemoveEventParticipant>()
                .SubscribeEvent<MediaFileDeleted>()
                .SubscribeEvent<EventImageUploaded>();

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