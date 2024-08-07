using Convey;
using Convey.Logging;
using Convey.Metrics.AppMetrics;
using Convey.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;
using System.Threading.Tasks;

namespace MiniSpace.APIGateway.Ocelot
{
    public static class Program
    {
        public static Task Main(string[] args) =>
            CreateHostBuilder(args).Build().RunAsync();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.AddJsonFile("general-ocelot.json", optional: false, reloadOnChange: true);
                        config.AddJsonFile("services/identity-service.json", optional: false, reloadOnChange: true);
                        config.AddJsonFile("services/reports-service.json", optional: false, reloadOnChange: true);
                        config.AddJsonFile("services/notifications-service.json", optional: false, reloadOnChange: true);
                        config.AddJsonFile("services/students-service.json", optional: false, reloadOnChange: true);
                        config.AddJsonFile("services/events-service.json", optional: false, reloadOnChange: true);
                        config.AddJsonFile("services/comments-service.json", optional: false, reloadOnChange: true);
                        config.AddJsonFile("services/reactions-service.json", optional: false, reloadOnChange: true);
                        config.AddJsonFile("services/friends-service.json", optional: false, reloadOnChange: true);
                        config.AddJsonFile("services/posts-service.json", optional: false, reloadOnChange: true);
                        config.AddJsonFile("services/mediafiles-service.json", optional: false, reloadOnChange: true);
                        config.AddJsonFile("services/organizations-service.json", optional: false, reloadOnChange: true);
                    });

                    webBuilder.ConfigureServices((context, services) =>
                    {
                        var key = Encoding.ASCII.GetBytes("Gtn9vBDB5RCDLJSMqZQQmN75J8hgzbQwWkcD8jMIXnvCLAmlL0QVacUAbyootWihMrPIz");
                        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                            {
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = false,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    ValidIssuer = "minispace",
                                    IssuerSigningKey = new SymmetricSecurityKey(key)
                                };
                            });

                        services.AddEndpointsApiExplorer();
                        services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MiniSpace API Gateway", Version = "v1" });
                        });

                        services.AddOcelot();
                        services.AddLogging();
                        services.AddConvey()
                            .AddMetrics()
                            .AddSecurity();
                    });

                    webBuilder.Configure(app =>
                    {
                        var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("DiscoveredRoutesLogger");
                        var ocelotConfig = app.ApplicationServices.GetRequiredService<IConfiguration>();

                        var configRoot = ocelotConfig as IConfigurationRoot;
                        if (configRoot != null)
                        {
                            logger.LogInformation("Loaded Ocelot Configuration: {Config}", configRoot.GetDebugView());
                        }
                        else
                        {
                            logger.LogWarning("Unable to cast IConfiguration to IConfigurationRoot.");
                        }

                        LogDiscoveredRoutes(ocelotConfig, logger);
                        
                        app.UseSwagger();
                        app.UseSwaggerUI(c =>
                        {
                            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MiniSpace API Gateway V1");
                            c.RoutePrefix = string.Empty;
                        });

                        app.UseRouting(); // Ensure UseRouting is called when using authentication and authorization.
                        app.UseAuthentication();
                        app.UseAuthorization(); // Add UseAuthorization if you have authorization policies.

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers(); // Ensure controllers are mapped if you use MVC controllers.
                        });

                        app.UseOcelot().Wait();
                    });
                })
                .UseLogging();

        private static void LogDiscoveredRoutes(IConfiguration configuration, ILogger logger)
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
