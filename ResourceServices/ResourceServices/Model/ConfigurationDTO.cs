using AuthenServices.Models;
using Newtonsoft.Json;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class ConfigurationDTO
    {
        public int configId { get; set; }
        public int companyId { get; set; }
        public string title { get; set; }
        public DateTime createDate { get; set; }
        public DateTime startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int duration { get; set; }
        public bool isActive { get; set; }
        public bool type { get; set; }
        public List<CatalogueInConfigurationDTO> catalogueInConfiguration;
        public List<RankInConfigDTO> rankInConfig;

        public ConfigurationDTO()
        {

        }
        public ConfigurationDTO(Configuration configuration)
        {
            this.configId = configuration.ConfigId;
            this.companyId = configuration.CompanyId;
            this.title = configuration.Title;
            this.createDate = configuration.CreateDate;
            this.startDate = configuration.StartDate;
            this.endDate = configuration.EndDate;
            this.duration = configuration.Duration;
            this.type = configuration.Type;
            this.isActive = configuration.IsActive;
            this.catalogueInConfiguration = configuration.CatalogueInConfiguration.Select(x => new CatalogueInConfigurationDTO(x)).ToList();
            this.rankInConfig = configuration.RankInConfig.Select(x => new RankInConfigDTO(x)).ToList();
        }
    }
}
