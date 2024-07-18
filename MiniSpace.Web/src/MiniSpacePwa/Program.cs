using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniSpacePwa;
using MiniSpacePwa.Areas.Comments;
using MiniSpacePwa.Areas.Events;
using MiniSpacePwa.Areas.Friends;
using MiniSpacePwa.Areas.Http;
using MiniSpacePwa.Areas.Identity;
using MiniSpacePwa.Areas.MediaFiles;
using MiniSpacePwa.Areas.Notifications;
using MiniSpacePwa.Areas.Organizations;
using MiniSpacePwa.Areas.Posts;
using MiniSpacePwa.Areas.Reactions;
using MiniSpacePwa.Areas.Reports;
using MiniSpacePwa.Areas.Students;
using MiniSpacePwa.HttpClients;
using System;
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

// Add local storage and other services
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<Radzen.DialogService, Radzen.DialogService>();
builder.Services.AddScoped<Radzen.DialogService, Radzen.DialogService>();
builder.Services.AddScoped<Radzen.NotificationService>(); 
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IStudentsService, StudentsService>();
builder.Services.AddScoped<IEventsService, EventsService>();
builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<IMediaFilesService, MediaFilesService>();
builder.Services.AddScoped<IOrganizationsService, OrganizationsService>();
builder.Services.AddScoped<IErrorMapperService, ErrorMapperService>();
builder.Services.AddScoped<IFriendsService, FriendsService>();
builder.Services.AddScoped<INotificationsService, NotificationsService>();
builder.Services.AddScoped<SignalRService>();
builder.Services.AddScoped<IReactionsService, ReactionsService>();
builder.Services.AddScoped<ICommentsService, CommentsService>();
builder.Services.AddScoped<IReportsService, ReportsService>();

// Add MudServices
builder.Services.AddMudServices();

await builder.Build().RunAsync();
