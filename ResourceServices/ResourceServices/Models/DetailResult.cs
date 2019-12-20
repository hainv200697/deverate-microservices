using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class DetailResult
    {
        public int TestId { get; set; }
        public int CatalogueInConfigId { get; set; }
        public double Point { get; set; }
        public bool IsActive { get; set; }

        public virtual CatalogueInConfiguration CatalogueInConfig { get; set; }
        public virtual Test Test { get; set; }
    }
}
