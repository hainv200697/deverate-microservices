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
        public int expiredDays { get; set; }
        public int duration { get; set; }
        public bool isActive { get; set; }
        public bool type { get; set; }
        public List<CatalogueInConfigurationDTO> catalogueInConfigurations;
        public List<RankInConfigDTO> rankInConfigs;

        public ConfigurationDTO()
        {

        }
        public ConfigurationDTO(Semester configuration)
        {
            this.configId = configuration.SemesterId;
            this.companyId = configuration.CompanyId;
            this.title = configuration.Title;
            this.createDate = configuration.CreateDate;
            this.expiredDays = configuration.ExpiredDays;
            this.duration = configuration.Duration;
            this.type = configuration.Type;
            this.isActive = configuration.IsActive;
            this.catalogueInConfigurations = configuration.CatalogueInSemester.Select(x => new CatalogueInConfigurationDTO(x)).ToList();
            this.rankInConfigs = configuration.RankInSemester.Select(x => new RankInConfigDTO(x)).ToList();
        }
    }
}
