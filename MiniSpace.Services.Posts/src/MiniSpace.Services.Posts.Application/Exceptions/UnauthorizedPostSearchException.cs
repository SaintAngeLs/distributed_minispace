namespace MiniSpace.Services.Posts.Application.Exceptions
{
    public class UnauthorizedPostSearchException : AppException
    {
        public override string Code { get; } = "unauthorized_post_search";
        public Guid StudentId { get; }
        public Guid IdentityId { get; }

        public UnauthorizedPostSearchException(Guid studentId, Guid identityId) : base("Unauthorized post search.")
        {
            StudentId = studentId;
            IdentityId = identityId;
        }
    }
}