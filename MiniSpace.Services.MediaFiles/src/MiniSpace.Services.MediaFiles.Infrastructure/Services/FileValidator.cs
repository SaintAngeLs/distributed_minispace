using System.Globalization;
using MiniSpace.Services.MediaFiles.Application.Exceptions;
using MiniSpace.Services.MediaFiles.Application.Services;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Services
{
    public class FileValidator : IFileValidator
    {
        private const int MaxFileSize = 5_000_000;

        private readonly Dictionary<string, string> _mimeTypes = new Dictionary<string, string>()
        {
            { "FFD8FFDB", "image/jpeg" },
            { "FFD8FFE0", "image/jpeg" },
            { "FFD8FFE1", "image/jpeg" },
            { "FFD8FFE2", "image/jpeg" },
            { "FFD8FFEE", "image/jpeg" },
            { "89504E47", "image/png" }, 
            { "47494638", "image/gif" }, 
            { "49492A00", "image/tiff" },
            { "4D4D002A", "image/tiff" },
            { "52494646", "image/webp" }, // Some formats use the first 8 bytes, e.g., "52494646" + "57454250" for WEBP
            { "57454250", "image/webp" },
            { "00000100", "image/ico" },
            { "00000200", "image/ico" }
        };

        public void ValidateFileSize(int size)
        {
            if (size > MaxFileSize)
            {
                throw new InvalidFileSizeException(size, MaxFileSize);
            }
        }

        public void ValidateFileExtensions(byte[] bytes, string contentType)
        {
            if (!_mimeTypes.ContainsValue(contentType))
            {
                throw new InvalidFileContentTypeException(contentType);
            }

            string hex = BitConverter.ToString(bytes, 0, 4).Replace("-", string.Empty).ToUpper(CultureInfo.InvariantCulture);
            if (bytes.Length >= 8)
            {
                // If available, try an 8-byte hex signature as well
                string extendedHex = BitConverter.ToString(bytes, 0, 8).Replace("-", string.Empty).ToUpper(CultureInfo.InvariantCulture);
                if (_mimeTypes.TryGetValue(extendedHex, out var extendedMimeType))
                {
                    if (extendedMimeType == contentType)
                    {
                        return; // Matched with 8-byte signature
                    }
                }
            }

            if (_mimeTypes.TryGetValue(hex, out var mimeType))
            {
                if (mimeType != contentType)
                {
                    throw new FileTypeDoesNotMatchContentTypeException(mimeType, contentType);
                }
            }
            else
            {
                throw new InvalidFileContentTypeException(contentType);
            }
        }
    }
}
