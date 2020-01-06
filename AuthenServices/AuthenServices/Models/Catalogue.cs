using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class Catalogue
    {
        public Catalogue()
        {
            CatalogueInConfiguration = new HashSet<CatalogueInConfiguration>();
            CatalogueInRank = new HashSet<CatalogueInRank>();
            Question = new HashSet<Question>();
        }

        public int CatalogueId { get; set; }
        public int? CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public bool? IsDefault { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<CatalogueInConfiguration> CatalogueInConfiguration { get; set; }
        public virtual ICollection<CatalogueInRank> CatalogueInRank { get; set; }
        public virtual ICollection<Question> Question { get; set; }
    }
}
