using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class RankInSemester
    {
        public int RankId { get; set; }
        public int SemesterId { get; set; }
        public bool IsActive { get; set; }
        public double Point { get; set; }

        public virtual Rank Rank { get; set; }
        public virtual Semester Semester { get; set; }
    }
}
