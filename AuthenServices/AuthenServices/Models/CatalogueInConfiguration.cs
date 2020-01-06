using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class CatalogueInConfiguration
    {
        public CatalogueInConfiguration()
        {
            DetailResult = new HashSet<DetailResult>();
        }

        public int CatalogueInConfigId { get; set; }
        public int ConfigId { get; set; }
        public int CatalogueId { get; set; }
        public double WeightPoint { get; set; }
        public int NumberQuestion { get; set; }
        public bool IsActive { get; set; }

        public virtual Catalogue Catalogue { get; set; }
        public virtual Configuration Config { get; set; }
        public virtual ICollection<DetailResult> DetailResult { get; set; }
    }
}
