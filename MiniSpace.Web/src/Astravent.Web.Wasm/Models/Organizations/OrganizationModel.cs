using System;
using System.Collections.Generic;

namespace Astravent.Web.Wasm.Models.Organizations
{
    public class OrganizationModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid RootId { get; set; }
        public OrganizationModel Parent { get; set; }
        public List<OrganizationModel> Children { get; set; }
        public bool Expanded { get; set; }
    }
}
