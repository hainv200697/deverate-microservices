using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class Account
    {
        public Account()
        {
            AccountInTest = new HashSet<AccountInTest>();
        }

        public int AccountId { get; set; }
        public int? CompanyId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool? Gender { get; set; }
        public string Avatar { get; set; }
        public DateTime? JoinDate { get; set; }
        public int RoleId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Company Company { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<AccountInTest> AccountInTest { get; set; }
    }
}
