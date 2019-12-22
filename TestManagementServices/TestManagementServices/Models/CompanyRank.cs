using System;
using System.Collections.Generic;

namespace TestManagementServices.Models
{
    public partial class CompanyRank
    {
        public CompanyRank()
        {
            Account = new HashSet<Account>();
            CatalogueInRank = new HashSet<CatalogueInRank>();
            Test = new HashSet<Test>();
        }

        public int CompanyRankId { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public int Position { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<CatalogueInRank> CatalogueInRank { get; set; }
        public virtual ICollection<Test> Test { get; set; }
    }
}
