using System.Collections.Generic;
using System.Threading.Tasks;
using Convey;
using Convey.Logging;
using Convey.Types;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Communication.Application;
using MiniSpace.Services.Communication.Application.Services;
using MiniSpace.Services.Communication.Application.Commands;
using MiniSpace.Services.Communication.Application.Dto;
using MiniSpace.Services.Communication.Application.Queries;
using MiniSpace.Services.Communication.Infrastructure;
using MiniSpace.Services.Communication.Application.Hubs;
using MiniSpace.Services.Communication.Core.Wrappers;
using System;

namespace MiniSpace.Services.Communication.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddConvey()
                            .AddWebApi()
                            .AddApplication()
                            .AddInfrastructure();
                    services.AddCors(options =>
                    {
                        options.AddPolicy("CorsPolicy",
                            builder => builder
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .SetIsOriginAllowed((host) => true));
                    });
                    services.AddSignalR();
                    services.AddAuthentication(); 
                    services.AddAuthorization(); 
                })
                .Configure(app => app
                    .UseInfrastructure()
                    .UseRouting()
                    .UseCors("CorsPolicy")
                    .UseAuthentication()
                    .UseAuthorization()
                    .UseEndpoints(endpoints =>
                    {
                        endpoints.MapHub<ChatHub>("/chatHub").RequireCors("CorsPolicy");
                    })
                    .UseDispatcherEndpoints(endpoints => endpoints
                        
                        // Chat-related endpoints
                        .Get<GetUserChats, PagedResponse<UserChatDto>>("communication/chats/user/{userId}")
                        .Get<GetChatById, ChatDto>("communication/chats/{chatId}")
                        .Get<GetMessagesForChat, IEnumerable<MessageDto>>("communication/chats/{chatId}/messages")
                        .Post<CreateChat>("communication/chats", async (cmd, ctx) =>
                        {
                            try
                            {
                                var chatId = await ctx.RequestServices.GetService<ICommunicationService>()
                                    .CreateChatAsync(cmd.ChatId, cmd.ParticipantIds, cmd.ChatName);
                                ctx.Response.StatusCode = StatusCodes.Status200OK;
                                await ctx.Response.WriteJsonAsync(new { ChatId = chatId });
                            }
                            catch (Exception ex)
                            {
                                ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
                                await ctx.Response.WriteJsonAsync(new { Error = ex.Message });
                            }
                        })

                        .Put<AddUserToChat>("communication/chats/{chatId}/users")
                        .Delete<DeleteChat>("communication/chats/{chatId}/{userId}")
                        
                        // Message-related endpoints
                        .Post<SendMessage>("communication/chats/{chatId}/messages")
                        .Put<UpdateMessageStatus>("communication/chats/{chatId}/messages/{messageId}/status")
                        .Delete<DeleteMessage>("communication/chats/{chatId}/messages/{messageId}") 
                    ))
                .UseLogging()
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
