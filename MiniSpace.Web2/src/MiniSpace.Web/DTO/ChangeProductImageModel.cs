
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Forms;

namespace MiniSpace.Web.DTO
{
    public class ChangeProductImageModel
    {
        public IReadOnlyList<IBrowserFile>? Files { get; set; }
    }
}
