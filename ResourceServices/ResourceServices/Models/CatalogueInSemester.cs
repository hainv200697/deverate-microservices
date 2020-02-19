using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class CatalogueInSemester
    {
        public CatalogueInSemester()
        {
            DetailResult = new HashSet<DetailResult>();
        }

        public int CatalogueInSemesterId { get; set; }
        public int SemesterId { get; set; }
        public int CatalogueId { get; set; }
        public double WeightPoint { get; set; }
        public int NumberQuestion { get; set; }
        public bool IsActive { get; set; }

        public virtual Catalogue Catalogue { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual ICollection<DetailResult> DetailResult { get; set; }
    }
}
