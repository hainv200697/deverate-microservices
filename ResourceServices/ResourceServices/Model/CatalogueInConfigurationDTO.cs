using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    [JsonObject("CatalogueInConfigurations", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CatalogueInConfigurationDTO
    {
        public int catalogueInConfigId { get; set; }
        public int configId { get; set; }
        public int catalogueId { get; set; }
        public double weightPoint { get; set; }
        public int numberQuestion { get; set; }
        public bool? isActive { get; set; }
        public List<CatalogueInRankDTO> catalogueInRankDTO;

        public CatalogueInConfigurationDTO()
        {

        }

        public CatalogueInConfigurationDTO(CatalogueInConfiguration catalogueInConfiguration)
        {
            this.catalogueInConfigId = catalogueInConfiguration.CatalogueInConfigId;
            this.configId = catalogueInConfiguration.ConfigId;
            this.catalogueId = catalogueInConfiguration.CompanyCatalogueId;
            this.weightPoint = catalogueInConfiguration.WeightPoint;
            this.numberQuestion = catalogueInConfiguration.NumberQuestion;
            this.isActive = catalogueInConfiguration.IsActive;
            catalogueInRankDTO = catalogueInConfiguration.CatalogueInRank.Select(x => new CatalogueInRankDTO(x)).ToList();
        }
    }
}
