using System.Globalization;
using MiniSpace.Services.MediaFiles.Application.Exceptions;
using MiniSpace.Services.MediaFiles.Application.Services;
using System.IO;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Services
{
    public class FileValidator : IFileValidator
    {
        private const long MaxFileSize = 5_000_000;

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
            { "52494646", "image/webp" },
            { "57454250", "image/webp" },
            { "00000100", "image/ico" },
            { "00000200", "image/ico" },
            { "25504446", "application/pdf" }, 
            { "504B0304", "application/zip" }, 
            { "00000018", "video/mp4" },
            { "00000020", "video/mp4" }, 
            { "1A45DFA3", "video/webm" },
            { "000001BA", "video/mpeg" }, 
        };


        public void ValidateFileSize(long size)
        {
            if (size > MaxFileSize)
            {
                throw new InvalidFileSizeException(size, MaxFileSize);
            }
        }

        public void ValidateFileExtensions(byte[] bytes, string contentType)
        {
            string hex = BitConverter.ToString(bytes, 0, 4).Replace("-", string.Empty).ToUpper(CultureInfo.InvariantCulture);
            if (bytes.Length >= 8)
            {
                string extendedHex = BitConverter.ToString(bytes, 0, 8).Replace("-", string.Empty).ToUpper(CultureInfo.InvariantCulture);
                if (_mimeTypes.TryGetValue(extendedHex, out var extendedMimeType))
                {
                    if (AreMimeTypesCompatible(extendedMimeType, contentType))
                    {
                        return; 
                    }
                }
            }

            if (_mimeTypes.TryGetValue(hex, out var mimeType))
            {
                if (!AreMimeTypesCompatible(mimeType, contentType))
                {
                    throw new FileTypeDoesNotMatchContentTypeException(mimeType, contentType);
                }
            }
            else
            {
                throw new InvalidFileContentTypeException(contentType);
            }
        }

        private bool AreMimeTypesCompatible(string detectedMimeType, string providedMimeType)
        {
            return string.Equals(detectedMimeType, providedMimeType, StringComparison.InvariantCultureIgnoreCase) ||
                   (detectedMimeType == "image/jpeg" && providedMimeType == "image/jpg") ||
                   (detectedMimeType == "image/jpg" && providedMimeType == "image/jpeg");
        }
    }
}
