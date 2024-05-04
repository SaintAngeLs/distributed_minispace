namespace MiniSpace.Services.Friends.Application.Dto
{
    public class FriendEventsDto
    {
        public Guid FriendId { get; set; }
        public IEnumerable<Guid> CommonEvents { get; set; }
    }
}
