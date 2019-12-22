using System;
using System.Collections.Generic;

namespace ResourceServices.Models
{
    public partial class DefaultRank
    {
        public int DefaultRankId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public int Position { get; set; }
    }
}
