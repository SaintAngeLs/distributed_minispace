using System.Linq;
using System.Text.RegularExpressions;

namespace MiniSpace.Services.Email.Core.Entities
{
    public class TwoFactorCodeGeneratedContent : IEmailContentStrategy
    {
        public string GenerateContent(string details)
        {
            var code = ExtractCode(details);
            if (string.IsNullOrEmpty(code))
            {
                return "<p>Invalid 2FA code provided.</p>";
            }
            var formattedCode = string.Join(" ", code.Select(c => $"<span style='border:1px solid #000;padding:5px;'>{c}</span>"));
            return $"<p>Your Two-Factor Authentication Code is:</p><div style='text-align: center;'>{formattedCode}</div>";
        }

        private string ExtractCode(string details)
        {
            var match = Regex.Match(details, @"\b\d{6}\b");
            return match.Success ? match.Value : string.Empty;
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
