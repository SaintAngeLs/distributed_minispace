using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.HttpClients;
using Astravent.Web.Wasm.DTO.Wrappers;
using Astravent.Web.Wasm.Areas.Identity;
using Astravent.Web.Wasm.DTO.Communication;
using Astravent.Web.Wasm.Areas.Communication.CommandsDto;
using System.Linq;

namespace Astravent.Web.Wasm.Areas.Communication
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

        public async Task<ChatDto> FindExistingChatAsync(Guid userId, Guid friendId)
        {
            var userChatsResponse = await GetUserChatsAsync(userId, 1, 100); 

            if (userChatsResponse == null || !userChatsResponse.Items.Any())
                return null;

            // Loop through all the chats to find one with the friend
            foreach (var userChat in userChatsResponse.Items.SelectMany(u => u.Chats))
            {
                if (userChat.ParticipantIds.Contains(friendId))
                {
                    // Return the chat if a matching participant is found
                    return userChat;
                }
            }

            // Return null if no existing chat is found
            return null;
        }


        public async Task<ChatDto> GetChatByIdAsync(Guid chatId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<ChatDto>($"communication/chats/{chatId}");
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesForChatAsync(Guid chatId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.GetAsync<IEnumerable<MessageDto>>($"communication/chats/{chatId}/messages");
        }

        public async Task<HttpResponse<Guid>> CreateChatAsync(CreateChatCommand command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PostAsync<CreateChatCommand, Guid>("communication/chats", command);
        }

        public async Task AddUserToChatAsync(Guid chatId, Guid userId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.PutAsync<object>($"communication/chats/{chatId}/users", new { chatId, userId });
        }

         public async Task DeleteChatAsync(Guid chatId, Guid userId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            var command = new DeleteChatCommand(chatId, userId); 
            await _httpClient.DeleteAsync($"communication/chats/{chatId}/{userId}", command); 
        }


        public async Task<HttpResponse<object>> SendMessageAsync(SendMessageCommand command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PostAsync<SendMessageCommand, object>($"communication/chats/{command.ChatId}/messages", command);
        }

        public async Task<HttpResponse<object>> UpdateMessageStatusAsync(UpdateMessageStatusCommand command)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            return await _httpClient.PutAsync<UpdateMessageStatusCommand, object>($"communication/chats/{command.ChatId}/messages/{command.MessageId}/status", command);
        }

        public async Task DeleteMessageAsync(Guid chatId, Guid messageId)
        {
            string accessToken = await _identityService.GetAccessTokenAsync();
            _httpClient.SetAccessToken(accessToken);
            await _httpClient.DeleteAsync($"communication/chats/{chatId}/messages/{messageId}");
        }
    }
}
