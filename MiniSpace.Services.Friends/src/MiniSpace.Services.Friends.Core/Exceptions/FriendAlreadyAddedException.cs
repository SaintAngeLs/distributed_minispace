namespace MiniSpace.Services.Friends.Core.Exceptions
{
    public class FriendAlreadyAddedException : DomainException
    {
        public override string Code { get; } = "friend_already_added";

        public FriendAlreadyAddedException() : base("This friend is already added.")
        {
        }
    }
}
