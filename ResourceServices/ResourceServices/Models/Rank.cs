using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class Rank
    {
        public Rank()
        {
            Account = new HashSet<Account>();
            CatalogueInRank = new HashSet<CatalogueInRank>();
            RankInConfig = new HashSet<RankInConfig>();
            TestPotentialRank = new HashSet<Test>();
            TestRank = new HashSet<Test>();
        }

        public int RankId { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public int Position { get; set; }
        public bool IsDefault { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<CatalogueInRank> CatalogueInRank { get; set; }
        public virtual ICollection<RankInConfig> RankInConfig { get; set; }
        public virtual ICollection<Test> TestPotentialRank { get; set; }
        public virtual ICollection<Test> TestRank { get; set; }
    }
}
