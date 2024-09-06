namespace MiniSpace.Services.Communication.Core.Entities
{
    public class UserChats
    {
        public Guid UserId { get; private set; }
        public List<Chat> Chats { get; private set; }

        public UserChats(Guid userId)
        {
            UserId = userId;
            Chats = new List<Chat>();
        }

        public void AddChat(Chat chat)
        {
            Chats.Add(chat);
        }

        public Chat GetChatById(Guid chatId)
        {
            return Chats.FirstOrDefault(chat => chat.Id == chatId);
        }
    }
}
