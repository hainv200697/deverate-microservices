using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class RankInConfigDTO
    {
        public int rankId { get; set; }
        public int configId { get; set; }
        public bool isActive { get; set; }
        public double point { get; set; }
        public string rank { get; set; }

        public RankInConfigDTO()
        {

        }

        public RankInConfigDTO(RankInConfig rankInConfig)
        {
            this.rankId = rankInConfig.RankId;
            this.configId = rankInConfig.ConfigId;
            this.isActive = rankInConfig.IsActive;
            this.point = rankInConfig.Point;
            this.rank = rankInConfig.Rank.Name;
        }

    }
}
