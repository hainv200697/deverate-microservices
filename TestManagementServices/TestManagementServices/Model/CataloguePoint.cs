using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    [JsonObject("CataloguePointDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CataloguePointDTO
    {
        public int catalogueId { get; set; }
        public double cataloguePoint { get; set; }

        public CataloguePointDTO() { }
        public CataloguePointDTO(int catalogueId, double cataloguePoint)
        {
            this.catalogueId = catalogueId;
            this.cataloguePoint = cataloguePoint;
        }
    }
}
