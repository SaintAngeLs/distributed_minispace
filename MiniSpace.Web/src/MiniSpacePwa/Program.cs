using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MiniSpacePwa;
using MiniSpacePwa.HttpClients;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient
var httpClientOptions = new HttpClientOptions();
builder.Configuration.Bind("HttpClientOptions", httpClientOptions);
builder.Services.AddSingleton(httpClientOptions);
builder.Services.AddHttpClient<IHttpClient, CustomHttpClient>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<HttpClientOptions>();
    client.BaseAddress = new Uri(options.ApiUrl);
});

// Add MudServices
builder.Services.AddMudServices();

await builder.Build().RunAsync();
