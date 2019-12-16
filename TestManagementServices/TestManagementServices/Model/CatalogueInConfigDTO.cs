using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    [JsonObject("CatalogueInConfigDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CatalogueInConfigDTO
    {
        public int catalogueInConfigId { get; set; }
        public int? configId { get; set; }
        public int? companyCatalogueId { get; set; }
        public string companyCatalogueName { get; set; }
        public double? weightPoint { get; set; }
        public bool? isActive { get; set; }
        public CatalogueInConfigDTO() { }
        public CatalogueInConfigDTO(CatalogueInConfiguration c)
        {
            this.catalogueInConfigId = c.CatalogueInConfigId;
            this.configId = c.ConfigId;
            this.companyCatalogueId = c.CompanyCatalogueId;
            this.companyCatalogueName = c.CompanyCatalogue.Name;
            this.weightPoint = c.WeightPoint;
            this.isActive = c.IsActive;
        }
    }
}
