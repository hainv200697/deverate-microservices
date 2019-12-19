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
        public int companyCatalogueId { get; set; }
        public double companyCataloguePoint { get; set; }

        public CataloguePointDTO() { }
        public CataloguePointDTO(int companyCatalogueId, double companyCataloguePoint)
        {
            this.companyCatalogueId = companyCatalogueId;
            this.companyCataloguePoint = companyCataloguePoint;
        }
    }
}
