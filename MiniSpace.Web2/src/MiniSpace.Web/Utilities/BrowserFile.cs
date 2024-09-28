using Microsoft.AspNetCore.Components.Forms;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSpace.Web.Utilities
{
    public class BrowserFile : IBrowserFile
    {
        private readonly byte[] _buffer;

        public BrowserFile(byte[] buffer, string name, string contentType, DateTimeOffset lastModified)
        {
            _buffer = buffer;
            Name = name;
            ContentType = contentType;
            LastModified = lastModified;
        }

        public string Name { get; }
        public string ContentType { get; }
        public long Size => _buffer.Length;

        // New property for LastModified
        public DateTimeOffset LastModified { get; }

        public Stream OpenReadStream(long maxAllowedSize = 10 * 1024 * 1024, CancellationToken cancellationToken = default)
        {
            return new MemoryStream(_buffer);
        }
    }
}
