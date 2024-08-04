namespace MiniSpace.Services.Email.Core.Entities
{
    public interface IEmailContentStrategy
    {
        string GenerateContent(string details);
    }
}
