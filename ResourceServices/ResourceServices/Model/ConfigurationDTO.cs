using AuthenServices.Models;
using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    [JsonObject("ConfigurationDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ConfigurationDTO
    {
        [JsonProperty("ConfigId")]
        public int ConfigId { get; set; }
        public int? TestOwnerId { get; set; }
        public int? TotalQuestion { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Duration { get; set; }
        public bool? IsActive { get;set; }
        public List<CatalogueInConfiguration> catalogueInConfigurations { get; set; }
        public List<ConfigurationRank> ConfigurationRank { get; set; }

        public ConfigurationDTO()
        {

        }

        public ConfigurationDTO(Configuration configuration)
        {
            this.ConfigId = configuration.ConfigId;
            this.TestOwnerId = configuration.TestOwnerId;
            this.TotalQuestion = configuration.TotalQuestion;
            this.CreateDate = configuration.CreateDate;
            this.StartDate = configuration.StartDate.Value.AddHours(7);
            this.EndDate = configuration.EndDate;
            this.Duration = configuration.Duration;
            this.IsActive = configuration.IsActive;
        }

        public ConfigurationDTO(Configuration configuration, List<CatalogueInConfiguration> catalogueInConfiguration, List<ConfigurationRank> configurationRank)
        {
            this.ConfigId = configuration.ConfigId;
            this.TestOwnerId = configuration.TestOwnerId;
            this.TotalQuestion = configuration.TotalQuestion;
            this.CreateDate = configuration.CreateDate;
            this.StartDate = configuration.StartDate.Value.AddHours(7);
            this.EndDate = configuration.EndDate.Value.AddHours(7);
            this.Duration = configuration.Duration;
            this.IsActive = configuration.IsActive;
            this.catalogueInConfigurations = catalogueInConfiguration;
            this.ConfigurationRank = configurationRank;
        }

  
    }
}
