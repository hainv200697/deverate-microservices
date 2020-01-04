using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    public class ConfigurationRankDTO
    {
        public int rankId { get; set; }
        public string rank { get; set; }
        public int position { get; set; }
        public double point { get; set; }

        public ConfigurationRankDTO() { }
        public ConfigurationRankDTO(int rankId, string rank,  double weightPoint, int position)
        {
            this.rankId = rankId;
            this.rank = rank;
            this.point = weightPoint;
            this.position = position;

        }
    }
}
