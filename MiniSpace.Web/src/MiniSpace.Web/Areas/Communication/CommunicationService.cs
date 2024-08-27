using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.DTO;
using MiniSpace.Web.HttpClients;
using MiniSpace.Web.DTO.Wrappers;
using MiniSpace.Web.Areas.Identity;
using MiniSpace.Web.DTO.Communication;
using MiniSpace.Web.Areas.Communication.CommandsDto;

namespace MiniSpace.Web.Areas.Communication
{
    public class CommunicationService : ICommunicationService
    {
        private readonly IHttpClient _httpClient;
        private readonly IIdentityService _identityService;

        public CommunicationService(IHttpClient httpClient, IIdentityService identityService)
        {
            _httpClient = httpClient;
            _identityService = identityService;
        }

        public async Task<PagedResponseDto<UserChatDto>> GetUserChatsAsync(Guid userId, int page, int pageSize)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return await _httpClient.GetAsync<PagedResponseDto<UserChatDto>>($"communication/chats/user/{userId}?page={page}&pageSize={pageSize}");
        }

        public async Task<ChatDto> GetChatByIdAsync(Guid chatId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return await _httpClient.GetAsync<ChatDto>($"communication/chats/{chatId}");
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesForChatAsync(Guid chatId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return await _httpClient.GetAsync<IEnumerable<MessageDto>>($"communication/chats/{chatId}/messages");
        }

        public async Task<HttpResponse<object>> CreateChatAsync(CreateChatCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return await _httpClient.PostAsync<CreateChatCommand, object>("communication/chats", command);
        }

        public async Task AddUserToChatAsync(Guid chatId, Guid userId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            await _httpClient.PutAsync<object>($"communication/chats/{chatId}/users", new { chatId, userId });
        }

        public async Task DeleteChatAsync(Guid chatId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            await _httpClient.DeleteAsync($"communication/chats/{chatId}");
        }

        public async Task<HttpResponse<object>> SendMessageAsync(SendMessageCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return await _httpClient.PostAsync<SendMessageCommand, object>($"communication/chats/{command.ChatId}/messages", command);
        }

        public async Task<HttpResponse<object>> UpdateMessageStatusAsync(UpdateMessageStatusCommand command)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            return await _httpClient.PutAsync<UpdateMessageStatusCommand, object>($"communication/chats/{command.ChatId}/messages/{command.MessageId}/status", command);
        }

        public async Task DeleteMessageAsync(Guid chatId, Guid messageId)
        {
            _httpClient.SetAccessToken(_identityService.JwtDto.AccessToken);
            await _httpClient.DeleteAsync($"communication/chats/{chatId}/messages/{messageId}");
        }
    }
}
