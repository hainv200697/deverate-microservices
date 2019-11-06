using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class CatalogueInCompany
    {
        public CatalogueInCompany()
        {
            Question = new HashSet<Question>();
        }

        public int Cicid { get; set; }
        public int CompanyId { get; set; }
        public int CatalogueId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Catalogue Catalogue { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Question> Question { get; set; }
    }
}
