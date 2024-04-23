using Convey.CQRS.Commands;

namespace MiniSpace.Services.Friends.Application.Events
{
    public class InviteFriend : ICommand
    {
        public Guid InviterId { get; }
        public Guid InviteeId { get; }

        public InviteFriend(Guid inviterId, Guid inviteeId)
        {
            InviterId = inviterId;
            InviteeId = inviteeId;
        }
    }
}
