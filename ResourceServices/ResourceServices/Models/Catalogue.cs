using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class Catalogue
    {
        public Catalogue()
        {
            CatalogueInConfiguration = new HashSet<CatalogueInConfiguration>();
            CatalogueInRank = new HashSet<CatalogueInRank>();
            CompanyCatalogue = new HashSet<CompanyCatalogue>();
            DetailStatistic = new HashSet<DetailStatistic>();
            Question = new HashSet<Question>();
        }

        public int CatalogueId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<CatalogueInConfiguration> CatalogueInConfiguration { get; set; }
        public virtual ICollection<CatalogueInRank> CatalogueInRank { get; set; }
        public virtual ICollection<CompanyCatalogue> CompanyCatalogue { get; set; }
        public virtual ICollection<DetailStatistic> DetailStatistic { get; set; }
        public virtual ICollection<Question> Question { get; set; }
    }
}
