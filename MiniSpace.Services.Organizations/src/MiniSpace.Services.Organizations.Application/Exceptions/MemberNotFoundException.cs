namespace MiniSpace.Services.Organizations.Application.Exceptions
{
    public class MemberNotFoundException : AppException
    {
        public override string Code { get; } = "member_not_found";
        public Guid MemberId { get; }

        public MemberNotFoundException(Guid memberId) : base($"Member with ID: {memberId} was not found.")
        {
            MemberId = memberId;
        }
    }
}
