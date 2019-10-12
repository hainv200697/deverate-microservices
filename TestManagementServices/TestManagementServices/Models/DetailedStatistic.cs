using System;
using System.Collections.Generic;

namespace TestManagementServices.Models
{
    public partial class DetailedStatistic
    {
        public int StatisticId { get; set; }
        public int? TestId { get; set; }
        public string RankId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Test Test { get; set; }
    }
}
