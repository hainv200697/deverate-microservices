using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class RankInConfiguration
    {
        public RankInConfiguration()
        {
            CatalogueInConfigRank = new HashSet<CatalogueInConfigRank>();
        }

        public int ConfigId { get; set; }
        public int CompanyRankId { get; set; }
        public double Point { get; set; }
        public bool IsActive { get; set; }

        public virtual CompanyRank CompanyRank { get; set; }
        public virtual Configuration Config { get; set; }
        public virtual ICollection<CatalogueInConfigRank> CatalogueInConfigRank { get; set; }
    }
}
