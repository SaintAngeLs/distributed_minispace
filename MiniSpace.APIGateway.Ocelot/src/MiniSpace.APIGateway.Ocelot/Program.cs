using System;
using System.Text;
using System.Threading.Tasks;
using Convey;
using Convey.Auth;
using Convey.Logging;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Tracing.Jaeger;
using Convey.Security;
using Convey.WebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using MiniSpace.APIGateway.Ocelot.Infrastructure;

namespace MiniSpace.APIGateway.Ocelot
{
    public static class Program
    {
        public static Task Main(string[] args) =>
            CreateHostBuilder(args).Build().RunAsync();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                          .AddJsonFile("general-ocelot.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"services/identity-service.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"services/reports-service.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"services/notifications-service.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"services/students-service.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"services/events-service.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"services/comments-service.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"services/reactions-service.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"services/friends-service.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"services/posts-service.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"services/mediafiles-service.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"services/organizations-service.json", optional: false, reloadOnChange: true)
                          .AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((context, services) =>
                    {
                        var keyString = context.Configuration["jwt:issuerSigningKey"];
                        if (string.IsNullOrEmpty(keyString))
                        {
                            throw new InvalidOperationException("JWT Key is not configured properly.");
                        }
                        var key = Encoding.ASCII.GetBytes(keyString);

                        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                            {
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = false,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    ValidIssuer = context.Configuration["jwt:validIssuer"],
                                    IssuerSigningKey = new SymmetricSecurityKey(key)
                                };
                            });

                        services.AddEndpointsApiExplorer();
                        services.AddSwaggerGen();
                        services.AddOcelot().AddPolly();

                        services
                            .AddConvey()
                            .AddErrorHandler<ExceptionToResponseMapper>()
                            .AddSecurity()
                            .AddJaeger()
                            // .AddMessageBrokers()
                            .AddRabbitMq()
                            .AddWebApi();

                        services.AddSingleton<IPayloadBuilder, PayloadBuilder>();
                        services.AddSingleton<ICorrelationContextBuilder, CorrelationContextBuilder>();
                        services.AddSingleton<IAnonymousRouteValidator, AnonymousRouteValidator>();
                        services.AddTransient<AsyncRoutesMiddleware>();
                        services.AddTransient<ResourceIdGeneratorMiddleware>();
                    });

                    webBuilder.Configure(app =>
                    {
                        var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("DiscoveredRoutesLogger");
                        var configRoot = app.ApplicationServices.GetService<IConfiguration>() as IConfigurationRoot;

                        if (configRoot != null)
                        {
                            logger.LogInformation("Loaded Ocelot Configuration: {Config}", configRoot.GetDebugView());
                        }
                        else
                        {
                            logger.LogWarning("Unable to cast IConfiguration to IConfigurationRoot.");
                        }

                        LogDiscoveredRoutes(configRoot, logger);

                        app.UseSwagger();
                        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MiniSpace API Gateway V1"));

                        app.UseRouting();
                        app.UseAuthentication();
                        app.UseAuthorization();

                        app.UseEndpoints(endpoints => endpoints.MapControllers());

                        app.UseMiddleware<AsyncRoutesMiddleware>();
                        app.UseOcelot().Wait();
                    })
                    .UseLogging();
                });

        private static void LogDiscoveredRoutes(IConfigurationRoot configuration, ILogger logger)
        {
            var routes = configuration.GetSection("Routes").GetChildren();
            foreach (var route in routes)
            {
                var upstreamPathTemplate = route.GetValue<string>("UpstreamPathTemplate");
                var downstreamPathTemplate = route.GetValue<string>("DownstreamPathTemplate");
                var methods = route.GetSection("UpstreamHttpMethod").GetChildren().Select(m => m.Value).ToArray();
                var methodsList = string.Join(", ", methods);

                logger.LogInformation("Discovered Route: Upstream Path Template: {UpstreamPathTemplate}, Methods: {Methods}, Downstream Path Template: {DownstreamPathTemplate}",
                    upstreamPathTemplate, methodsList, downstreamPathTemplate);
            }
        }
    }
}
