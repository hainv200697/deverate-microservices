using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class CatalogueInRank
    {
        public int CatalogueInConfigId { get; set; }
        public int CompanyRankId { get; set; }
        public double? Point { get; set; }
        public bool? IsActive { get; set; }

        public virtual CatalogueInConfiguration CatalogueInConfig { get; set; }
        public virtual CompanyRank CompanyRank { get; set; }
    }
}
