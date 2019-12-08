using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class ConfigurationCatalogueDTO
    {
        public int configurationCatalogueId { get; set; }
        public int configId { get; set; }
        public int catalogueId { get; set; }
        public double? weightPoint { get; set; }
        public bool? isActive { get; set; }
        public CatalogueDTO catalogue { get; set; }
        public ConfigurationDTO configuration { get; set; }

        public ConfigurationCatalogueDTO()
        {

        }

        public ConfigurationCatalogueDTO(CatalogueInConfiguration catalogueInConfiguration)
        {
            this.configId = catalogueInConfiguration.ConfigId;
            this.catalogueId = catalogueInConfiguration.CompanyCatalogueId;
            this.weightPoint = catalogueInConfiguration.WeightPoint;
            this.isActive = catalogueInConfiguration.IsActive;
        }

        public ConfigurationCatalogueDTO(CatalogueInConfiguration catalogueInConfiguration, Configuration configuration, CompanyCatalogue catalogue)
        {
            this.configId = catalogueInConfiguration.ConfigId;
            this.catalogueId = catalogueInConfiguration.CompanyCatalogueId;
            this.weightPoint = catalogueInConfiguration.WeightPoint;
            this.isActive = catalogueInConfiguration.IsActive;
            this.configuration = new ConfigurationDTO(configuration);
            this.catalogue = new CatalogueDTO(catalogue);
        }
    }
}
