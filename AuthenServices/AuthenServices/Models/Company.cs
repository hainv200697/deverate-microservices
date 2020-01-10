using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class Company
    {
        public Company()
        {
            Account = new HashSet<Account>();
            Catalogue = new HashSet<Catalogue>();
            Configuration = new HashSet<Configuration>();
            Rank = new HashSet<Rank>();
        }

        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime CreateDate { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<Catalogue> Catalogue { get; set; }
        public virtual ICollection<Configuration> Configuration { get; set; }
        public virtual ICollection<Rank> Rank { get; set; }
    }
}
