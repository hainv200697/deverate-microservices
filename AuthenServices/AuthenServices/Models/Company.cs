using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class Company
    {
        public Company()
        {
            Account = new HashSet<Account>();
            CompanyCatalogue = new HashSet<CompanyCatalogue>();
            CompanyRank = new HashSet<CompanyRank>();
        }

        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime CreateDate { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<CompanyCatalogue> CompanyCatalogue { get; set; }
        public virtual ICollection<CompanyRank> CompanyRank { get; set; }
    }
}
