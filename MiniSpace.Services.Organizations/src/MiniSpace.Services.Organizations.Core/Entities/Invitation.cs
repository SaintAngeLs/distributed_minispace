namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class Invitation
    {
        public Guid UserId { get; }

        public Invitation(Guid userId)
        {

            UserId = userId;
        }
    }
}
