using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Astravent.Web.Wasm.DTO
{
    public class StudentWithGalleryImagesDto
    {
        public StudentDto Student { get; set; }
        public List<GalleryImageDto> GalleryImages { get; set; }
    }
}
