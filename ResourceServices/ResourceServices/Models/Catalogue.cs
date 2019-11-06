using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class Catalogue
    {
        public Catalogue()
        {
            CatalogueInCompany = new HashSet<CatalogueInCompany>();
            CatalogueInConfiguration = new HashSet<CatalogueInConfiguration>();
            CatalogueInRank = new HashSet<CatalogueInRank>();
            DetailStatistic = new HashSet<DetailStatistic>();
        }

        public int CatalogueId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? Type { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<CatalogueInCompany> CatalogueInCompany { get; set; }
        public virtual ICollection<CatalogueInConfiguration> CatalogueInConfiguration { get; set; }
        public virtual ICollection<CatalogueInRank> CatalogueInRank { get; set; }
        public virtual ICollection<DetailStatistic> DetailStatistic { get; set; }
    }
}
