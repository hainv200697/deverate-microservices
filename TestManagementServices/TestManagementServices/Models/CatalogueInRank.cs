using System;
using System.Collections.Generic;

namespace TestManagementServices.Models
{
    public partial class CatalogueInRank
    {
        public int Cirid { get; set; }
        public int? ConfigurationRankId { get; set; }
        public int? CatalogueId { get; set; }
        public double? WeightPoint { get; set; }
        public bool? IsActive { get; set; }

        public virtual Catalogue Catalogue { get; set; }
        public virtual ConfigurationRank ConfigurationRank { get; set; }
    }
}
