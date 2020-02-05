using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class Configuration
    {
        public Configuration()
        {
            CatalogueInConfiguration = new HashSet<CatalogueInConfiguration>();
            RankInConfig = new HashSet<RankInConfig>();
            Test = new HashSet<Test>();
        }

        public int ConfigId { get; set; }
        public int CompanyId { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public int Duration { get; set; }
        public bool Type { get; set; }
        public bool IsActive { get; set; }
        public int ExpiredDays { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<CatalogueInConfiguration> CatalogueInConfiguration { get; set; }
        public virtual ICollection<RankInConfig> RankInConfig { get; set; }
        public virtual ICollection<Test> Test { get; set; }
    }
}
