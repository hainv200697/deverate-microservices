using System;
using System.Collections.Generic;

namespace MailingServices.Models
{
    public partial class Statistic
    {
        public Statistic()
        {
            DetailStatistic = new HashSet<DetailStatistic>();
        }

        public int StatisticId { get; set; }
        public int TestId { get; set; }
        public double Point { get; set; }
        public int RankId { get; set; }
        public bool IsActive { get; set; }

        public virtual Rank Rank { get; set; }
        public virtual Test Test { get; set; }
        public virtual ICollection<DetailStatistic> DetailStatistic { get; set; }
    }
}
