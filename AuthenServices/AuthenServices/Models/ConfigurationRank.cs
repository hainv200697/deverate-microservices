using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class ConfigurationRank
    {
        public int ConfigurationRankId { get; set; }
        public int RankId { get; set; }
        public double? WeightPoint { get; set; }
        public bool? IsActive { get; set; }

        public virtual Configuration ConfigurationRankNavigation { get; set; }
        public virtual Rank Rank { get; set; }
    }
}
