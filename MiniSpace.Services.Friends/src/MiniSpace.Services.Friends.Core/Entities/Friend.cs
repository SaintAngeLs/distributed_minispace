using System;
using MiniSpace.Services.Friends.Core.Events;
using MiniSpace.Services.Friends.Core.Exceptions;

namespace MiniSpace.Services.Friends.Core.Entities
{
    public class Friend : AggregateRoot
    {
        public Guid FriendId { get; private set; }
        public Guid StudentId { get; private set; }
        public State FriendState { get; private set; }
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
            FriendState = State.Unknown;
            ValidateState();
        }

        public void SendFriendRequest(Student requester, Student requestee)
        {
            if (FriendState != State.Unknown)
            {
                throw new InvalidFriendStateException("Friend request cannot be sent from the current state.");
            }
            FriendState = State.Requested;
            AddEvent(new FriendRequestCreated(requester, requestee));
        }

        public void InviteFriend(Student inviter, Student invitee)
        {
            AddEvent(new FriendInvited(inviter, invitee));
        }

        public void AcceptFriendship(Student requester, Student acceptor)
        {
            if (FriendState != State.Requested) 
            {
                throw new FriendshipStateException(StudentId, State.Accepted, FriendState);
            }
            FriendState = State.Accepted;
            AddEvent(new PendingFriendAccepted(requester, acceptor));
        }

        public void DeclineFriendship(Student requester, Student decliner)
        {
            if (FriendState != State.Requested)
            {
                throw new FriendDeclinationException(StudentId, FriendId);
            }
            FriendState = State.Declined;
            AddEvent(new PendingFriendDeclined(requester, decliner));
        }

        public void ChangeFriendshipState(Student student, State newState)
        {
            if (FriendState == newState)
            {
                throw new FriendshipStateException(StudentId, newState, FriendState);
            }
            State previousState = FriendState;
            FriendState = newState;
            AddEvent(new FriendshipStateChanged(student, previousState));
        }

        protected override void ValidateState()
        {
            if (StudentId == Guid.Empty || FriendId == Guid.Empty)
            {
                throw new InvalidFriendRequestException(StudentId, FriendId);
            }

            if (StudentId == FriendId)
            {
                throw new InvalidFriendInvitationException(StudentId, FriendId);
            }
        }
    }
}
