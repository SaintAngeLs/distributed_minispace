using System;
using System.Linq;
using System.Threading.Tasks;
using Paralax;
using Paralax.Logging;
using Paralax.Metrics.AppMetrics;
using Paralax.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nuar;
using Nuar.RabbitMq;
using Nuar.Hooks;
using MiniSpace.APIGateway.Infrastructure;
using Microsoft.Extensions.Logging;

namespace MiniSpace.APIGateway
{
    public static class Program
    {
        public static Task Main(string[] args)
            => CreateHostBuilder(args).Build().RunAsync();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");
                    webBuilder.CaptureStartupErrors(true);
                    webBuilder.ConfigureAppConfiguration(builder =>
                    {
                        const string extension = "yml";
                        var ntradaConfig = Environment.GetEnvironmentVariable("NTRADA_CONFIG");
                        var configPath = args?.FirstOrDefault() ?? ntradaConfig ?? $"nuar.{extension}";
                        if (!configPath.EndsWith($".{extension}"))
                        {
                            configPath += $".{extension}";
                        }

                        builder.AddYamlFile(configPath, false);
                    })
                    .ConfigureServices(services => services.AddNuar()
                        .AddSingleton<IContextBuilder, CorrelationContextBuilder>()
                        .AddSingleton<ISpanContextBuilder, SpanContextBuilder>()
                        .AddSingleton<IHttpRequestHook, HttpRequestHook>()
                        .AddParalax()
                        .AddMetrics()
                        .AddSecurity())
                    .Configure(app =>
                    {
                        app.UseNuar(); 
                    })
                    .UseLogging();
                });

    }
}
