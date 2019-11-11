using System;
using System.Collections.Generic;

namespace TestManagementServices.Models
{
    public partial class Role
    {
        public Role()
        {
            Account = new HashSet<Account>();
        }

        public int RoleId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Account> Account { get; set; }
    }
}
