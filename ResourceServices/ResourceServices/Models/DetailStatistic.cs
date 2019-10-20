using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class DetailStatistic
    {
        public int DetailId { get; set; }
        public int? StatisticId { get; set; }
        public int? CatalogueId { get; set; }
        public double? Point { get; set; }
        public bool? IsActive { get; set; }

        public virtual Catalogue Catalogue { get; set; }
        public virtual Statistic Statistic { get; set; }
    }
}
