﻿using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class DetailedStatistic
    {
        public int StatisticId { get; set; }
        public int? Aitid { get; set; }
        public string RankId { get; set; }
        public bool? IsActive { get; set; }

        public virtual AccountInTest Ait { get; set; }
    }
}
