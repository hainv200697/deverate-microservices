﻿using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class RankInConfig
    {
        public int RankId { get; set; }
        public int ConfigId { get; set; }
        public bool IsActive { get; set; }

        public virtual Configuration Config { get; set; }
        public virtual Rank Rank { get; set; }
    }
}