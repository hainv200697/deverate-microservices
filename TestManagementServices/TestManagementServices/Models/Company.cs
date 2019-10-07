using System;
using System.Collections.Generic;

namespace TestManagementServices.Models
{
    public partial class Company
    {
        public Company()
        {
            Account = new HashSet<Account>();
            CompanyCatalogue = new HashSet<CompanyCatalogue>();
        }

        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime? CreateAt { get; set; }
        public bool? IsActive { get; set; }
        public int? Phone { get; set; }
        public string Fax { get; set; }

        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<CompanyCatalogue> CompanyCatalogue { get; set; }
    }
}
