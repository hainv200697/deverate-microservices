using System;
using System.Collections.Generic;

namespace MailingServices.Models
{
    public partial class Role
    {
        public Role()
        {
            Account = new HashSet<Account>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Account> Account { get; set; }
    }
}
