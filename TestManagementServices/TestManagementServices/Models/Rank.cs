using System;
using System.Collections.Generic;

namespace TestManagementServices.Models
{
    public partial class Rank
    {
        public Rank()
        {
            Account = new HashSet<Account>();
            CatalogueInRank = new HashSet<CatalogueInRank>();
            RankInSemester = new HashSet<RankInSemester>();
            TestPotentialRank = new HashSet<Test>();
            TestRank = new HashSet<Test>();
        }

        public int RankId { get; set; }
        public int? CompanyId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<CatalogueInRank> CatalogueInRank { get; set; }
        public virtual ICollection<RankInSemester> RankInSemester { get; set; }
        public virtual ICollection<Test> TestPotentialRank { get; set; }
        public virtual ICollection<Test> TestRank { get; set; }
    }
}
