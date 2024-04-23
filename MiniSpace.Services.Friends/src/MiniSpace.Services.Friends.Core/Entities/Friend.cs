using System;
using MiniSpace.Services.Friends.Core.Events;
using MiniSpace.Services.Friends.Core.Exceptions;

namespace MiniSpace.Services.Friends.Core.Entities
{
    public class Friend : AggregateRoot
    {
        public Guid FriendId { get; private set; }
        public Guid StudentId { get; private set; }
        public FriendState FriendState { get; private set; }
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string FullName => $"{FirstName} {LastName}";
        public DateTime CreatedAt { get; private set; }

        public Friend(Guid studentId, Guid friendId, string email, string firstName, string lastName, DateTime createdAt)
        {
            Id = new AggregateId(Guid.NewGuid());
            StudentId = studentId;
            FriendId = friendId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            CreatedAt = createdAt;
            FriendState = FriendState.Unknown;
        }

        public void InviteFriend(Student inviter, Student invitee)
        {
            if (FriendState != FriendState.Unknown)
            {
                throw new InvalidFriendInvitationException(inviter.Id, invitee.Id);
            }
            FriendState = FriendState.Requested;
            AddEvent(new FriendInvited(this, new Friend(invitee.Id, this.FriendId, invitee.FullName, invitee.FullName, "", DateTime.UtcNow)));
        }

        public void AcceptFriendship(Student friend)
        {
            if (FriendState != FriendState.Requested)
            {
                throw new InvalidFriendStateException(FriendId, "Friendship cannot be accepted in the current state.");
            }
            FriendState = FriendState.Accepted;
            AddEvent(new FriendAdded(new Student(StudentId, FullName), friend));
        }

        public void MarkAsConfirmed()
        {
            if (FriendState != FriendState.Requested)
                throw new InvalidFriendshipStateException(Id, FriendState.ToString(), "Requested");

            FriendState = FriendState.Confirmed;
            AddEvent(new FriendshipConfirmed(FriendId));
        }



        public void RemoveFriend(Student friend)
        {
            if (FriendState != FriendState.Accepted)
            {
                throw new InvalidFriendStateException(FriendId, "Only accepted friendships can be removed.");
            }
            FriendState = FriendState.Cancelled;
            AddEvent(new FriendRemoved(new Student(StudentId, FullName), friend));
        }
    }
}
