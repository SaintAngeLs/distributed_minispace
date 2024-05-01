using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniSpace.Web.Areas.Events;
using MiniSpace.Web.Areas.Http;
//using MiniSpace.Web.Data;
using MiniSpace.Web.Models.Identity;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.Areas.Posts;
using MiniSpace.Web.Areas.Students;
using MiniSpace.Web.HttpClients;
using MudBlazor;
using MudBlazor.Services;
using MiniSpace.Web.Areas.Friends;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

namespace MiniSpace.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddMudServices();

            var httpClientOptions = Configuration.GetSection("HttpClientOptions").Get<HttpClientOptions>();
    
            // Register HttpClientOptions as a singleton
            services.AddSingleton(httpClientOptions);

            // Register IHttpClient to resolve to CustomHttpClient
            services.AddHttpClient<IHttpClient, CustomHttpClient>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<HttpClientOptions>();
                client.BaseAddress = new Uri(options.ApiUrl); 
            });

            services.AddBlazoredLocalStorage(); 


            services.AddScoped<Radzen.DialogService, Radzen.DialogService>();
            
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IStudentsService, StudentsService>();
            services.AddScoped<IEventsService, EventsService>();
            services.AddScoped<IPostsService, PostsService>();
            services.AddScoped<IErrorMapperService, ErrorMapperService>();
            services.AddScoped<IFriendsService, FriendsService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

        }
    }
}
