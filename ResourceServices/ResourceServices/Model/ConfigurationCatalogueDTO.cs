using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Model
{
    public class ConfigurationCatalogueDTO
    {
        public int ConfigurationCatalogueId { get; set; }
        public int ConfigId { get; set; }
        public int CatalogueId { get; set; }
        public double? WeightPoint { get; set; }
        public bool? IsActive { get; set; }
        public CatalogueDTO catalogue { get; set; }
        public ConfigurationDTO configuration { get; set; }

        public ConfigurationCatalogueDTO()
        {

        }

        public ConfigurationCatalogueDTO(CatalogueInConfiguration catalogueInConfiguration)
        {
            this.ConfigurationCatalogueId = catalogueInConfiguration.Cicid;
            this.ConfigId = catalogueInConfiguration.ConfigId;
            this.CatalogueId = catalogueInConfiguration.CatalogueId;
            this.WeightPoint = catalogueInConfiguration.WeightPoint;
            this.IsActive = catalogueInConfiguration.IsActive;
        }

        public ConfigurationCatalogueDTO(CatalogueInConfiguration catalogueInConfiguration, Configuration configuration, Catalogue catalogue)
        {
            this.ConfigurationCatalogueId = catalogueInConfiguration.Cicid;
            this.ConfigId = catalogueInConfiguration.ConfigId;
            this.CatalogueId = catalogueInConfiguration.CatalogueId;
            this.WeightPoint = catalogueInConfiguration.WeightPoint;
            this.IsActive = catalogueInConfiguration.IsActive;
            this.configuration = new ConfigurationDTO(configuration);
            this.catalogue = new CatalogueDTO(catalogue);
        }
    }
}
