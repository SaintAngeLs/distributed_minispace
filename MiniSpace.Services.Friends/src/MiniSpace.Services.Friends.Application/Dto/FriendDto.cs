using MiniSpace.Services.Friends.Core.Entities;

namespace MiniSpace.Services.Friends.Application.Dto
{
    public class FriendDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public Guid StudentId { get; set; }
        public Guid FriendId { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public DateTime CreatedAt { get; set; }
        public FriendState State { get; set; }
    }
}
