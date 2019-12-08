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
        public string rank { get; set; }
        public List<CatalogueInRankDTO> catalogueInRanks { get; set; }
        

        public ConfigurationRankDTO()
        {

        }

        public ConfigurationRankDTO(RankInConfiguration configurationRank)
        {
            this.configId = configurationRank.ConfigId;
            this.rankId = configurationRank.CompanyRankId;
            this.weightPoint = configurationRank.Point;
            this.isActive = configurationRank.IsActive;
            this.rank = configurationRank.CompanyRank.Name;
            List<CatalogueInRankDTO> catalogueIns = new List<CatalogueInRankDTO>();
            //configurationRank.CatalogueInRank.ToList().ForEach(c => catalogueIns.Add(new CatalogueInRankDTO(c.ConfigurationRank.RankId, c.ConfigurationRank.Rank.Name, c.Catalogue)))
            this.catalogueInRanks = configurationRank.CatalogueInConfigRank.Select(c => new CatalogueInRankDTO(c)).ToList();
        }

        public ConfigurationRankDTO(RankInConfiguration configurationRank, Configuration configuration, CompanyRank rank)
        {
            this.configId = configurationRank.ConfigId;
            this.rankId = configurationRank.CompanyRankId;
            this.weightPoint = configurationRank.Point;
            this.isActive = configurationRank.IsActive;
            this.configuration = new ConfigurationDTO(configuration);
            this.rank = configurationRank.CompanyRank.Name;
        }
    }
}
