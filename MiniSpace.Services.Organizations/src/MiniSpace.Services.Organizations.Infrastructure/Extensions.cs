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
using MiniSpace.Services.Organizations.Application;
using MiniSpace.Services.Organizations.Application.Events.External;
using MiniSpace.Services.Organizations.Application.Commands;
using MiniSpace.Services.Organizations.Application.Services;
using MiniSpace.Services.Organizations.Core.Repositories;
using MiniSpace.Services.Organizations.Infrastructure.Contexts;
using MiniSpace.Services.Organizations.Infrastructure.Decorators;
using MiniSpace.Services.Organizations.Infrastructure.Exceptions;
using MiniSpace.Services.Organizations.Infrastructure.Logging;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Repositories;
using MiniSpace.Services.Organizations.Infrastructure.Services;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static IParalaxBuilder AddInfrastructure(this IParalaxBuilder builder)
        {
            builder.Services.AddTransient<IOrganizationRepository, OrganizationMongoRepository>();
            builder.Services.AddScoped<IOrganizationReadOnlyRepository, OrganizationMongoRepository>();
            builder.Services.AddTransient<IOrganizationGalleryRepository, OrganizationGalleryMongoRepository>();
            builder.Services.AddTransient<IOrganizationMembersRepository, OrganizationMembersMongoRepository>();
            builder.Services.AddTransient<IUserInvitationsRepository, UserInvitationsMongoRepository>();
            builder.Services.AddTransient<IOrganizationRolesRepository, OrganizationRolesMongoRepository>();
            builder.Services.AddTransient<IOrganizationRequestsRepository, OrganizationRequestsMongoRepository>();
            builder.Services.AddTransient<IUserOrganizationsRepository, UserOrganizationsMongoRepository>();

            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddSingleton<IEventMapper, EventMapper>();
            builder.Services.AddTransient<IMessageBroker, MessageBroker>();
            builder.Services.AddTransient<IAppContextFactory, AppContextFactory>();
            builder.Services.AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>().Create());
            builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
            builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));

            builder.Services.AddGrpc();

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
                .AddMongoRepository<OrganizationDocument, Guid>("organizations")
                .AddMongoRepository<OrganizationGalleryImageDocument, Guid>("organization_gallery_images")
                .AddMongoRepository<OrganizationMembersDocument, Guid>("organization_members")
                .AddMongoRepository<OrganizationInvitationDocument, Guid>("organization_invitations")
                .AddMongoRepository<OrganizationRolesDocument, Guid>("organization_roles")
                .AddMongoRepository<OrganizationRequestsDocument, Guid>("organization_requests")
                .AddMongoRepository<UserOrganizationsDocument, Guid>("user_organizations")
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
                .SubscribeCommand<CreateOrganization>()
                .SubscribeCommand<CreateSubOrganization>()
                .SubscribeCommand<CreateOrganizationRole>()
                .SubscribeCommand<DeleteOrganization>()
                .SubscribeCommand<InviteUserToOrganization>()
                .SubscribeCommand<AssignRoleToMember>()
                .SubscribeCommand<UpdateRolePermissions>()
                .SubscribeCommand<SetOrganizationPrivacy>()
                .SubscribeCommand<UpdateOrganizationSettings>()
                .SubscribeCommand<SetOrganizationVisibility>()
                .SubscribeCommand<UpdateOrganization>()
                .SubscribeCommand<FollowOrganization>()
                .SubscribeCommand<AcceptFollowRequest>()
                .SubscribeCommand<RejectFollowRequest>()
                .SubscribeCommand<ManageFeed>()
                .SubscribeCommand<LeaveOrganization>()
                .SubscribeEvent<OrganizationImageUploaded>()
                .SubscribeEvent<MediaFileDeleted>();

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
