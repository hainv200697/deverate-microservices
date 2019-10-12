using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class Role
    {
        public int RoleId { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
