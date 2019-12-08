using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class CatalogueInConfigRank
    {
        public int ConfigId { get; set; }
        public int CompanyRankId { get; set; }
        public int CompanyCatalogueId { get; set; }
        public double Point { get; set; }
        public bool IsActive { get; set; }

        public virtual RankInConfiguration Co { get; set; }
        public virtual CompanyCatalogue CompanyCatalogue { get; set; }
    }
}
