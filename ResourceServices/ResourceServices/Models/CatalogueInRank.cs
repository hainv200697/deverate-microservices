using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class CatalogueInRank
    {
        public int CatalogueId { get; set; }
        public int RankId { get; set; }
        public double Point { get; set; }
        public bool IsActive { get; set; }

        public virtual Catalogue Catalogue { get; set; }
        public virtual Rank Rank { get; set; }
    }
}
