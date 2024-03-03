using Chronicle;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.Discovery.Consul;
using Convey.Docs.Swagger;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.AppMetrics;
using Convey.Persistence.Redis;
using Convey.Security;
using Convey.WebApi;
using Convey.WebApi.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.OrderMaker.Events.External;
using Pacco.Services.OrderMaker.Services;
using Pacco.Services.OrderMaker.Services.Clients;

namespace Pacco.Services.OrderMaker
{
    public static class Extensions
    {
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            builder
                .AddErrorHandler<ExceptionToResponseMapper>()
                .AddHttpClient()
                .AddConsul()
                .AddFabio()
                .AddCommandHandlers()
                .AddEventHandlers()
                .AddInMemoryCommandDispatcher()
                .AddInMemoryEventDispatcher()
                .AddRedis()
                .AddMetrics()
                .AddRabbitMq()
                .AddWebApiSwaggerDocs()
                .AddSecurity();

            builder.Services.AddChronicle();
            builder.Services.AddTransient<IAvailabilityServiceClient, AvailabilityServiceClient>();
            builder.Services.AddTransient<IVehiclesServiceClient, VehiclesServiceClient>();
            builder.Services.AddTransient<IResourceReservationsService, ResourceReservationsService>();
            
            return builder;
        }

        public static IApplicationBuilder UseApp(this IApplicationBuilder app)
        {
            app.UseErrorHandler()
                .UseSwaggerDocs()
                .UseConvey()
                .UseMetrics()
                .UseRabbitMq()
                .SubscribeEvent<OrderApproved>()
                .SubscribeEvent<OrderCreated>()
                .SubscribeEvent<ParcelAddedToOrder>()
                .SubscribeEvent<ResourceReserved>()
                .SubscribeEvent<VehicleAssignedToOrder>();

            return app;
        }
    }
}