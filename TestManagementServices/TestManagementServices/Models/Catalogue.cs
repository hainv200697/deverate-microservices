using System;
using System.Collections.Generic;

namespace TestManagementServices.Models
{
    public partial class Catalogue
    {
        public Catalogue()
        {
            CatalogueInRank = new HashSet<CatalogueInRank>();
            CatalogueInSemester = new HashSet<CatalogueInSemester>();
            Question = new HashSet<Question>();
        }

        public int CatalogueId { get; set; }
        public int? CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<CatalogueInRank> CatalogueInRank { get; set; }
        public virtual ICollection<CatalogueInSemester> CatalogueInSemester { get; set; }
        public virtual ICollection<Question> Question { get; set; }
    }
}
