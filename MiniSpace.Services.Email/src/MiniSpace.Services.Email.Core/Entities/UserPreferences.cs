namespace MiniSpace.Services.Email.Core.Entities
{
    public class UserPreferences
    {
        public Guid UserId { get; set; }
        public bool ReceivesEmailNotifications { get; set; }

        public UserPreferences(Guid userId, bool receivesEmailNotifications)
        {
            UserId = userId;
            ReceivesEmailNotifications = receivesEmailNotifications;
        }
    }
}
