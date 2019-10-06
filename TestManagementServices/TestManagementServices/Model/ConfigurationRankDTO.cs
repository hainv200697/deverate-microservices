using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    public class ConfigurationRankDTO
    {
        public int? configurationRankId { get; set; }
        public int? rankId { get; set; }
        public double? point { get; set; }
        public bool? isActive { get; set; }
        public ConfigurationRankDTO() { }
        public ConfigurationRankDTO(ConfigurationRank configuration)
        {
            this.configurationRankId = configuration.ConfigurationRankId;
            this.rankId = configuration.RankId;
            this.point = configuration.WeightPoint;
            this.isActive = configuration.IsActive;
        }
    }
}
