using System;
using System.Collections.Generic;

namespace TestManagementServices.Models
{
    public partial class DetailResult
    {
        public int TestId { get; set; }
        public int CatalogueInSemesterId { get; set; }
        public double Point { get; set; }
        public bool IsActive { get; set; }

        public virtual CatalogueInSemester CatalogueInSemester { get; set; }
        public virtual Test Test { get; set; }
    }
}
