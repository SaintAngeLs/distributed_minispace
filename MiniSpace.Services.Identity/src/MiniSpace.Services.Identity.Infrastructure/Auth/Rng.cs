using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using MiniSpace.Services.Identity.Application.Services;

namespace MiniSpace.Services.Identity.Infrastructure.Auth
{
    [ExcludeFromCodeCoverage]
    internal sealed class Rng : IRng
    {
        private static readonly string[] SpecialChars = new[] {"/", "\\", "=", "+", "?", ":", "&"};

        public string Generate(int length = 50, bool removeSpecialChars = true)
        {
            using var rng = new RNGCryptoServiceProvider();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            var result = Convert.ToBase64String(bytes);

            return removeSpecialChars
                ? SpecialChars.Aggregate(result, (current, chars) => current.Replace(chars, string.Empty))
                : result;
        }
    }
}