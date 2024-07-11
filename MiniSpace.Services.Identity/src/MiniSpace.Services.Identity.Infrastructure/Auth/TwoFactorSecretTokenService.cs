using MiniSpace.Services.Identity.Application.Services;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MiniSpace.Services.Identity.Infrastructure.Auth
{
    public class TwoFactorSecretTokenService : ITwoFactorSecretTokenService
    {
        private static readonly char[] Base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();

        public string GenerateSecret()
        {
            byte[] bytes = new byte[20];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }
            return ToBase32(bytes);
        }

        private static string ToBase32(byte[] bytes)
        {
            StringBuilder base32 = new StringBuilder((bytes.Length * 8 + 4) / 5);

            for (int i = 0; i < bytes.Length;)
            {
                int currentByte = bytes[i++];
                int digit;

                base32.Append(Base32Chars[(currentByte & 0xF8) >> 3]);
                digit = (currentByte & 0x07) << 2;
                if (i >= bytes.Length)
                {
                    base32.Append(Base32Chars[digit]);
                    break;
                }
                currentByte = bytes[i++];
                digit |= (currentByte & 0xC0) >> 6;
                base32.Append(Base32Chars[digit]);
                base32.Append(Base32Chars[(currentByte & 0x3E) >> 1]);
                digit = (currentByte & 0x01) << 4;
                if (i >= bytes.Length)
                {
                    base32.Append(Base32Chars[digit]);
                    break;
                }
                currentByte = bytes[i++];
                digit |= (currentByte & 0xF0) >> 4;
                base32.Append(Base32Chars[digit]);
                digit = (currentByte & 0x0F) << 1;
                if (i >= bytes.Length)
                {
                    base32.Append(Base32Chars[digit]);
                    break;
                }
                currentByte = bytes[i++];
                digit |= (currentByte & 0x80) >> 7;
                base32.Append(Base32Chars[digit]);
                base32.Append(Base32Chars[(currentByte & 0x7C) >> 2]);
                digit = (currentByte & 0x03) << 3;
                if (i >= bytes.Length)
                {
                    base32.Append(Base32Chars[digit]);
                    break;
                }
                currentByte = bytes[i++];
                digit |= (currentByte & 0xE0) >> 5;
                base32.Append(Base32Chars[digit]);
                base32.Append(Base32Chars[currentByte & 0x1F]);
            }

            return base32.ToString();
        }
    }
}

