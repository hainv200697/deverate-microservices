using System;
using System.Collections.Generic;

namespace AuthenServices.Models
{
    public partial class Rank
    {
        public Rank()
        {
            ConfigurationRank = new HashSet<ConfigurationRank>();
        }

        public int RankId { get; set; }
        public string Name { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<ConfigurationRank> ConfigurationRank { get; set; }
    }
}
