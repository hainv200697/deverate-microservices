using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class CompanyCatalogue
    {
        public CompanyCatalogue()
        {
            CatalogueInConfiguration = new HashSet<CatalogueInConfiguration>();
            Question = new HashSet<Question>();
        }

        public int CompanyCatalogueId { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<CatalogueInConfiguration> CatalogueInConfiguration { get; set; }
        public virtual ICollection<Question> Question { get; set; }
    }
}
