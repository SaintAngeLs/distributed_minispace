using System;
using System.Collections.Generic;

namespace MiniSpace.Web.Models.Organizations
{
    public class OrganizerModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool WasBelonging { get; set; }

        public OrganizerModel(Guid id, string email, string name)
        {
            Id = id;
            Email = email;
            Name = name;
        }
    }
}
