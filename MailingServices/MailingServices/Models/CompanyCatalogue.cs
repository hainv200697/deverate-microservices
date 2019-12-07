using MailingServices.Models;
using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class CompanyCatalogue
    {
        public int CompanyId { get; set; }
        public int CatalogueId { get; set; }
        public bool IsActive { get; set; }

        public virtual Catalogue Catalogue { get; set; }
        public virtual Company Company { get; set; }
    }
}
