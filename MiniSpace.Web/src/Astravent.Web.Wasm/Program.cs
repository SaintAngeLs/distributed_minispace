using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Astravent.Web.Wasm;
using Blazored.LocalStorage;
using MudBlazor.Services;
using Radzen;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System;
using Astravent.Web.Wasm.Areas.Http;
using Astravent.Web.Wasm.Models.Identity;
using Astravent.Web.Wasm.Areas.Events;
using Astravent.Web.Wasm.Areas.Friends;
using Astravent.Web.Wasm.Areas.Communication;
using Astravent.Web.Wasm.Areas.Posts;
using Astravent.Web.Wasm.Areas.Students;
using Astravent.Web.Wasm.Areas.Organizations;
using Astravent.Web.Wasm.Areas.Notifications;
using Astravent.Web.Wasm.Areas.MediaFiles;
using Astravent.Web.Wasm.Areas.Reactions;
using Astravent.Web.Wasm.Areas.Comments;
using Astravent.Web.Wasm.Areas.Reports;
using MudBlazor;
using Astravent.Web.Wasm.Areas.Identity;
using Astravent.Web.Wasm.HttpClients;
using Astravent.Web.Wasm.Utilities;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

if (builder.HostEnvironment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
}
else
{
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
}

var httpClientOptions = builder.Configuration.GetSection("HttpClientOptions").Get<HttpClientOptions>();

if (httpClientOptions == null || string.IsNullOrEmpty(httpClientOptions.ApiUrl))
{
    throw new InvalidOperationException("HttpClientOptions must be configured with a valid ApiUrl.");
}

builder.Services.AddSingleton(httpClientOptions);

builder.Services.AddHttpClient<IHttpClient, CustomHttpClient>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<HttpClientOptions>();
    client.BaseAddress = new Uri(options.ApiUrl);
});


builder.Services.AddMudServices(); 
builder.Services.AddMudMarkdownServices(); 
builder.Services.AddBlazoredLocalStorage(); 

builder.Services.AddScoped<NotificationService>();

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore(); 

builder.Services.AddScoped<IStudentsService, StudentsService>();
builder.Services.AddScoped<IEventsService, EventsService>();
builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<IMediaFilesService, MediaFilesService>();
builder.Services.AddScoped<IOrganizationsService, OrganizationsService>();
builder.Services.AddScoped<IErrorMapperService, ErrorMapperService>();
builder.Services.AddScoped<IFriendsService, FriendsService>();
builder.Services.AddScoped<INotificationsService, NotificationsService>();
builder.Services.AddScoped<IReactionsService, ReactionsService>();
builder.Services.AddScoped<ICommentsService, CommentsService>();
builder.Services.AddScoped<IReportsService, ReportsService>();
builder.Services.AddScoped<ICommunicationService, CommunicationService>();
builder.Services.AddScoped<IIPAddressService, IPAddressService>();

builder.Services.AddScoped<SignalRService>();
builder.Services.AddScoped<ChatSignalRService>();

await builder.Build().RunAsync();
