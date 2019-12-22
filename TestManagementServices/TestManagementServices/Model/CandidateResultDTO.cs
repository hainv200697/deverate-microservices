using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Model;

namespace TestManagementServices.Models
{
    [JsonObject("CandidateResultDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CandidateResultDTO
    {
        public int? accountId { get; set; }
        public List<ConfigurationRankDTO> configurationRanks { get; set; }
        public List<CatalogueInRankDTO> catalogueInRanks { get; set; }
        public List<CatalogueInConfigDTO> catalogueInConfigs { get; set; }
        public List<CompanyCatalogueDTO> catalogues { get; set; }
        public double? point { get; set; }
        public int? rankId { get; set; }
        public string rank { get; set; }
        public int? potentialRankId { get; set; }
        public string potentialRank { get; set; }
        public double lowerTestPercent { get; set; }

        public CandidateResultDTO() { }

        public CandidateResultDTO(int? accountId, List<ConfigurationRankDTO> configurationRanks,
            List<CompanyCatalogueDTO> catalogues, List<CatalogueInRankDTO> catalogueInRanks,
            List<CatalogueInConfigDTO> catalogueInConfigs, double? point, int? rankId, string rank,
            int? potentialRankId, string potentialRank, double lowerTestPercent)
        {
            this.accountId = accountId;
            this.configurationRanks = configurationRanks;
            this.catalogues = catalogues;
            this.catalogueInRanks = catalogueInRanks;
            this.point = point;
            this.rankId = rankId;
            this.rank = rank;
            this.catalogueInConfigs = catalogueInConfigs;
            this.potentialRankId = potentialRankId;
            this.potentialRank = potentialRank;
            this.lowerTestPercent = lowerTestPercent;
        }


    }


}
