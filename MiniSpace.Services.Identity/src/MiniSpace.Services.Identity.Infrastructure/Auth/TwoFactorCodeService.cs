using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MiniSpace.Services.Identity.Application.Services;

namespace MiniSpace.Services.Identity.Infrastructure.Auth
{
    public class TwoFactorCodeService : ITwoFactorCodeService
    {
        private const int Step = 30; // Time step in seconds
        private const int TOTPSize = 6; // Number of digits in the TOTP
        private const int Window = 1; // Allowable time step window for clock drift

        public bool ValidateCode(string secret, string code)
        {
            long unixTime = GetUnixTimestamp();
            byte[] secretBytes = Base32ToBytes(secret);
            

            for (int i = -Window; i <= Window; i++)
            {
                int otp = ComputeTotp(secretBytes, (unixTime / Step) + i);
                Console.WriteLine($"Generated OTP for step {i}: {otp.ToString("D6")}");

                if (otp.ToString("D6") == code)
                {
                    return true;
                }
            }

            return false;
        }

        public string GenerateCode(string secret)
        {
            byte[] secretBytes = Base32ToBytes(secret);
            long unixTime = GetUnixTimestamp();
            int otp = ComputeTotp(secretBytes, unixTime / Step);
            return otp.ToString("D6");
        }

        private static long GetUnixTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public static int ComputeTotp(byte[] key, long counter)
        {
            using (var hmac = new HMACSHA1(key))
            {
                byte[] counterBytes = BitConverter.GetBytes(counter);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(counterBytes);
                }

                byte[] hash = hmac.ComputeHash(counterBytes);
                int offset = hash[hash.Length - 1] & 0xf;
                int binary =
                    ((hash[offset] & 0x7f) << 24) |
                    ((hash[offset + 1] & 0xff) << 16) |
                    ((hash[offset + 2] & 0xff) << 8) |
                    (hash[offset + 3] & 0xff);

                int otp = binary % (int)Math.Pow(10, TOTPSize);
                return otp;
            }
        }

        public static byte[] Base32ToBytes(string base32)
        {
            const string base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            base32 = base32.TrimEnd('=').ToUpper();
            byte[] outputBytes = new byte[base32.Length * 5 / 8];

            byte curByte = 0, bitsRemaining = 8;
            int mask = 0, arrayIndex = 0;

            foreach (char c in base32)
            {
                int cValue = base32Chars.IndexOf(c);

                if (cValue < 0)
                {
                    throw new ArgumentException("Invalid base32 character", nameof(base32));
                }

                if (bitsRemaining > 5)
                {
                    mask = cValue << (bitsRemaining - 5);
                    curByte |= (byte)mask;
                    bitsRemaining -= 5;
                }
                else
                {
                    mask = cValue >> (5 - bitsRemaining);
                    curByte |= (byte)mask;
                    outputBytes[arrayIndex++] = curByte;
                    curByte = (byte)(cValue << (3 + bitsRemaining));
                    bitsRemaining += 3;
                }
            }

            if (arrayIndex != outputBytes.Length)
            {
                outputBytes[arrayIndex] = curByte;
            }

            return outputBytes;
        }
    }
}
