
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Forms;

namespace Astravent.Web.Wasm.DTO
{
    public class ChangeProductImageModel
    {
        public IReadOnlyList<IBrowserFile>? Files { get; set; }
    }
}
