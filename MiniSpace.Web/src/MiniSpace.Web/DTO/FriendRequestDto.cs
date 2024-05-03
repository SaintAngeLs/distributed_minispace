using System;
using MiniSpace.Web.DTO.States;

namespace MiniSpace.Services.Friends.Application.Dto
{
    public class FriendRequestDto
    {
        public Guid Id { get; set; }
        public Guid InviterId { get; set; }
        public Guid InviteeId { get; set; }
        public DateTime RequestedAt { get; set; }
        public FriendState State { get; set; }
        public Guid StudentId { get; set; }

        public string InviteeName { get; set; }
        public string InviteeEmail { get; set; }
        public string InviteeImage { get; set; }
    }
}