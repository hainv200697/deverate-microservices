using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class CompanyCatalogue
    {
        public CompanyCatalogue()
        {
            CatalogueInConfigRank = new HashSet<CatalogueInConfigRank>();
            CatalogueInConfiguration = new HashSet<CatalogueInConfiguration>();
            DetailResult = new HashSet<DetailResult>();
            Question = new HashSet<Question>();
        }

        public int CompanyCatalogueId { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreateDate { get; set; }
        public bool IsActive { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<CatalogueInConfigRank> CatalogueInConfigRank { get; set; }
        public virtual ICollection<CatalogueInConfiguration> CatalogueInConfiguration { get; set; }
        public virtual ICollection<DetailResult> DetailResult { get; set; }
        public virtual ICollection<Question> Question { get; set; }
    }
}
