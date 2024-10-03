using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Friends.Application.Commands
{
    public class InviteFriend : ICommand
    {
        public Guid InviterId { get; set; }
        public Guid InviteeId { get; set; }

        public InviteFriend(Guid inviterId, Guid inviteeId)
        {
            InviterId = inviterId;
            InviteeId = inviteeId;
        }
    }
}
