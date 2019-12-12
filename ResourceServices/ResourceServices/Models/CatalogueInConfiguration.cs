using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class CatalogueInConfiguration
    {
        public CatalogueInConfiguration()
        {
            CatalogueInRank = new HashSet<CatalogueInRank>();
            DetailResult = new HashSet<DetailResult>();
        }

        public int CatalogueInConfigId { get; set; }
        public int ConfigId { get; set; }
        public int CompanyCatalogueId { get; set; }
        public double WeightPoint { get; set; }
        public int NumberQuestion { get; set; }
        public bool IsActive { get; set; }

        public virtual CompanyCatalogue CompanyCatalogue { get; set; }
        public virtual Configuration Config { get; set; }
        public virtual ICollection<CatalogueInRank> CatalogueInRank { get; set; }
        public virtual ICollection<DetailResult> DetailResult { get; set; }
    }
}
