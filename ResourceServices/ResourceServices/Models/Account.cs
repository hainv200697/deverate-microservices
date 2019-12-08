﻿using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class Account
    {
        public Account()
        {
            Configuration = new HashSet<Configuration>();
            Test = new HashSet<Test>();
        }

        public int AccountId { get; set; }
        public int? CompanyId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
        public DateTime? JoinDate { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }

        public virtual Company Company { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Configuration> Configuration { get; set; }
        public virtual ICollection<Test> Test { get; set; }
    }
}
