using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Web.Areas.Communication.CommandsDto;
using MiniSpace.Web.DTO;
using MiniSpace.Web.DTO.Communication;
using MiniSpace.Web.DTO.Wrappers;
using MiniSpace.Web.HttpClients;

namespace MiniSpace.Web.Areas.Communication
{
    public interface ICommunicationService
    {
        Task<PagedResponseDto<UserChatDto>> GetUserChatsAsync(Guid userId, int page, int pageSize);
        Task<ChatDto> FindExistingChatAsync(Guid userId, Guid friendId);
        Task<ChatDto> GetChatByIdAsync(Guid chatId);
        Task<IEnumerable<MessageDto>> GetMessagesForChatAsync(Guid chatId);
        Task<HttpResponse<object>> CreateChatAsync(CreateChatCommand command);
        Task AddUserToChatAsync(Guid chatId, Guid userId);
        Task DeleteChatAsync(Guid chatId);
        Task<HttpResponse<object>> SendMessageAsync(SendMessageCommand command);
        Task<HttpResponse<object>> UpdateMessageStatusAsync(UpdateMessageStatusCommand command);
        Task DeleteMessageAsync(Guid chatId, Guid messageId);
    }
}
