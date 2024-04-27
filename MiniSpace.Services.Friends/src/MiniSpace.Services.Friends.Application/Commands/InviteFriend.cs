using Convey.CQRS.Commands;

namespace MiniSpace.Services.Friends.Application.Events
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
