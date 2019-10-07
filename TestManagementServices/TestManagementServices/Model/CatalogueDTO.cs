using AuthenServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    [JsonObject("CataloguesDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CatalogueDTO
    {
        public CatalogueDTO()
        {

        }

        public CatalogueDTO(Catalogue catalogue)
        {
            this.catalogueId = catalogue.CatalogueId;
            this.name = catalogue.Name;
            this.isActive = catalogue.IsActive;
        }

        public CatalogueDTO(int? catalogueId, string name, int? numberOfQuestion, double? weightPoint, bool? isActive)
        {
            this.catalogueId = catalogueId;
            this.name = name;
            this.numberOfQuestion = numberOfQuestion;
            this.weightPoint = weightPoint;
            this.isActive = isActive;
        }

        [JsonProperty("CatalogueId")]
        public int? catalogueId { get; set; }
        public string name { get; set; }
        public int? numberOfQuestion { get; set; }
        public double? weightPoint { get; set; }
        public bool? isActive { get; set; }
    }
}
