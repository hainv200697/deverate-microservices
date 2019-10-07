using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    public class RankPoint
    {
        public string rank { get; set; }
        public double? point { get; set; }
        public RankPoint() { }
        public RankPoint(string rank, double? point)
        {
            this.rank = rank;
            this.point = point;
        }
    }
}
