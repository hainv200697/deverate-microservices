using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class DetailResult
    {
        public int TestId { get; set; }
        public int CompanyCatalogueId { get; set; }
        public double Point { get; set; }
        public bool IsActive { get; set; }

        public virtual CompanyCatalogue CompanyCatalogue { get; set; }
        public virtual Test Test { get; set; }
    }
}
