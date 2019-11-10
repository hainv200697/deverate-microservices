using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class ConfigurationRankDTO
    {
        public int? configurationRankId { get; set; }
        public int? configId { get; set; }
        public int? rankId { get; set; }
        public double? weightPoint { get; set; }
        public bool? isActive { get; set; }
        public ConfigurationDTO configuration { get; set; }
        public RankDTO rank { get; set; }
        public List<CatalogueInRankDTO> catalogueInRanks { get; set; }
        

        public ConfigurationRankDTO()
        {

        }

        public ConfigurationRankDTO(ConfigurationRank configurationRank)
        {
            this.configurationRankId = configurationRank.ConfigurationRankId;
            this.configId = configurationRank.ConfigId;
            this.rankId = configurationRank.RankId;
            this.weightPoint = configurationRank.WeightPoint;
            this.isActive = configurationRank.IsActive;
            this.rank = new RankDTO(configurationRank.Rank);
            List<CatalogueInRankDTO> catalogueIns = new List<CatalogueInRankDTO>();
            //configurationRank.CatalogueInRank.ToList().ForEach(c => catalogueIns.Add(new CatalogueInRankDTO(c.ConfigurationRank.RankId, c.ConfigurationRank.Rank.Name, c.Catalogue)))
            this.catalogueInRanks = configurationRank.CatalogueInRank.Select(c => new CatalogueInRankDTO(c)).ToList();
        }

        public ConfigurationRankDTO(ConfigurationRank configurationRank, Configuration configuration, Rank rank)
        {
            this.configurationRankId = configurationRank.ConfigurationRankId;
            this.configId = configurationRank.ConfigId;
            this.rankId = configurationRank.RankId;
            this.weightPoint = configurationRank.WeightPoint;
            this.isActive = configurationRank.IsActive;
            this.configuration = new ConfigurationDTO(configuration);
            this.rank = new RankDTO(rank);
        }
    }
}
