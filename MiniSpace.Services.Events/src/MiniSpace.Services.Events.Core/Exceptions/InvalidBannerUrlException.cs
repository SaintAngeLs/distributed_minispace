using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Events.Core.Exceptions
{
    public class InvalidBannerUrlException : DomainException
    {
        public override string Code { get; } = "invalid_banner_url";

        public InvalidBannerUrlException(string bannerUrl)
            : base($"The banner URL '{bannerUrl}' is invalid.")
        {
        }
    }
}