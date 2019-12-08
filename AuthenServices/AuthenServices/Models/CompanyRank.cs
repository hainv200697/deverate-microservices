using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class CompanyRank
    {
        public CompanyRank()
        {
            RankInConfiguration = new HashSet<RankInConfiguration>();
            Test = new HashSet<Test>();
        }

        public int CompanyRankId { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<RankInConfiguration> RankInConfiguration { get; set; }
        public virtual ICollection<Test> Test { get; set; }
    }
}
