namespace MiniSpace.Services.Email.Core.Entities
{
    public interface IEmailSubjectStrategy
    {
        string GenerateSubject(string details);
    }
}
