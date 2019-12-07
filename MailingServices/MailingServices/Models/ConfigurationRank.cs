using System;
using System.Collections.Generic;

namespace MailingServices.Models
{
    public partial class ConfigurationRank
    {
        public ConfigurationRank()
        {
            CatalogueInRank = new HashSet<CatalogueInRank>();
        }

        public int ConfigurationRankId { get; set; }
        public int ConfigId { get; set; }
        public int RankId { get; set; }
        public double WeightPoint { get; set; }
        public bool IsActive { get; set; }

        public virtual Configuration Config { get; set; }
        public virtual Rank Rank { get; set; }
        public virtual ICollection<CatalogueInRank> CatalogueInRank { get; set; }
    }
}
