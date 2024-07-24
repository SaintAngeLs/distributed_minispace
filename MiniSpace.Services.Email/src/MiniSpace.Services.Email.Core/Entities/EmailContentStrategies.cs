namespace MiniSpace.Services.Email.Core.Entities
{
    public class TwoFactorCodeGeneratedContent : IEmailContentStrategy
    {
        public string GenerateContent(string code)
        {
            var formattedCode = string.Join(" ", code.Select(c => $"<span style='border:1px solid #000;padding:5px;'>{c}</span>"));
            return $"<p>Your Two-Factor Authentication Code is:</p><p>{formattedCode}</p>";
        }
    }

    public class DefaultEmailContentStrategy : IEmailContentStrategy
    {
        public string GenerateContent(string details)
        {
            return $"<p>{details}</p>";
        }
    }
}
