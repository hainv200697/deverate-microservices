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
        public int configId { get; set; }
        public int? testOwnerId { get; set; }
        public int? totalQuestion { get; set; }
        public DateTime? createDate { get; set; } = DateTime.Now;
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int? duration { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; } = true;
        public string TestOwnerName { get; set; }
        public List<CatalogueInConfiguration> catalogueInConfigurations { get; set; }
        public List<ConfigurationRank> ConfigurationRank { get; set; }


        public ConfigurationDTO()
        {

        }

        public ConfigurationDTO(Configuration configuration)
        {
            this.configId = configuration.ConfigId;
            this.testOwnerId = configuration.TestOwnerId;
            this.totalQuestion = configuration.TotalQuestion;
            this.createDate = configuration.CreateDate;
            this.startDate = configuration.StartDate.Value.AddHours(7);
            this.endDate = configuration.EndDate;
            this.duration = configuration.Duration;
            this.isActive = configuration.IsActive;
        }

        public ConfigurationDTO(Configuration configuration, List<CatalogueInConfiguration> catalogueInConfiguration, List<ConfigurationRank> configurationRank, string name)
        {
            this.configId = configuration.ConfigId;
            this.testOwnerId = configuration.TestOwnerId;
            this.totalQuestion = configuration.TotalQuestion;
            this.createDate = configuration.CreateDate;
            this.startDate = configuration.StartDate.Value.AddHours(7);
            this.endDate = configuration.EndDate.Value.AddHours(7);
            this.duration = configuration.Duration;
            this.isActive = configuration.IsActive;
            this.title = configuration.Title;
            this.TestOwnerName = name;
            this.catalogueInConfigurations = catalogueInConfiguration;
            this.ConfigurationRank = configurationRank;
        }

  
    }
}
