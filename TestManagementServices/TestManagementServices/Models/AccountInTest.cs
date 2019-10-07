using System;
using System.Collections.Generic;

namespace TestManagementServices.Models
{
    public partial class AccountInTest
    {
        public AccountInTest()
        {
            DetailedStatistic = new HashSet<DetailedStatistic>();
        }

        public int Aitid { get; set; }
        public int? AccountId { get; set; }
        public int? TestId { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; }

        public virtual Account Account { get; set; }
        public virtual Test Test { get; set; }
        public virtual ICollection<DetailedStatistic> DetailedStatistic { get; set; }
    }
}
