using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Astravent.Web.Wasm.Areas.Communication.CommandsDto;
using Astravent.Web.Wasm.DTO;
using Astravent.Web.Wasm.DTO.Communication;
using Astravent.Web.Wasm.DTO.Wrappers;
using Astravent.Web.Wasm.HttpClients;

namespace Astravent.Web.Wasm.Areas.Communication
{
    public interface ICommunicationService
    {
        Task<PagedResponseDto<UserChatDto>> GetUserChatsAsync(Guid userId, int page, int pageSize);
        Task<ChatDto> FindExistingChatAsync(Guid userId, Guid friendId);
        Task<ChatDto> GetChatByIdAsync(Guid chatId);
        Task<IEnumerable<MessageDto>> GetMessagesForChatAsync(Guid chatId);
        Task<HttpResponse<Guid>> CreateChatAsync(CreateChatCommand command);
        Task AddUserToChatAsync(Guid chatId, Guid userId);
        Task DeleteChatAsync(Guid chatId, Guid userId);
        Task<HttpResponse<object>> SendMessageAsync(SendMessageCommand command);
        Task<HttpResponse<object>> UpdateMessageStatusAsync(UpdateMessageStatusCommand command);
        Task DeleteMessageAsync(Guid chatId, Guid messageId);
    }
}
