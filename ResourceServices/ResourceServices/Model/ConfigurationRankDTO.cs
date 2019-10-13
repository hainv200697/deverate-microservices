using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class ConfigurationRankDTO
    {
        public int? ConfigurationRankId { get; set; }
        public int? ConfigId { get; set; }
        public int? RankId { get; set; }
        public double? WeightPoint { get; set; }
        public bool? IsActive { get; set; }
        public ConfigurationDTO configuration { get; set; }
        public RankDTO rank { get; set; }

        public ConfigurationRankDTO()
        {

        }

        public ConfigurationRankDTO(ConfigurationRank configurationRank)
        {
            this.ConfigurationRankId = configurationRank.ConfigurationRankId;
            this.ConfigId = configurationRank.ConfigId;
            this.RankId = configurationRank.RankId;
            this.WeightPoint = configurationRank.WeightPoint;
            this.IsActive = configurationRank.IsActive;
        }

        public ConfigurationRankDTO(ConfigurationRank configurationRank, Configuration configuration, Rank rank)
        {
            this.ConfigurationRankId = configurationRank.ConfigurationRankId;
            this.ConfigId = configurationRank.ConfigId;
            this.RankId = configurationRank.RankId;
            this.WeightPoint = configurationRank.WeightPoint;
            this.IsActive = configurationRank.IsActive;
            this.configuration = new ConfigurationDTO(configuration);
            this.rank = new RankDTO(rank);
        }
    }
}
