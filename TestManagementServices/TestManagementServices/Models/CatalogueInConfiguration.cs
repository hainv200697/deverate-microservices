using System;
using System.Collections.Generic;

namespace TestManagementServices.Models
{
    public partial class CatalogueInConfiguration
    {
        public int Cicid { get; set; }
        public int ConfigId { get; set; }
        public int CatalogueId { get; set; }
        public double? WeightPoint { get; set; }
        public bool? IsActive { get; set; }

        public virtual Catalogue Catalogue { get; set; }
        public virtual Configuration Config { get; set; }
    }
}
