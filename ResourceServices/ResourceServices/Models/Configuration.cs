using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class Configuration
    {
        public Configuration()
        {
            CatalogueInConfiguration = new HashSet<CatalogueInConfiguration>();
        }

        public int ConfigId { get; set; }
        public int TestId { get; set; }
        public int? TestOwnerId { get; set; }
        public int ConfigurationRankId { get; set; }
        public int? TotalQuestion { get; set; }
        public int? TimeTest { get; set; }
        public bool? IsActive { get; set; }

        public virtual Test Test { get; set; }
        public virtual ConfigurationRank ConfigurationRank { get; set; }
        public virtual ICollection<CatalogueInConfiguration> CatalogueInConfiguration { get; set; }
    }
}
