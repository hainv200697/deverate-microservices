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
        public int? catalogueId { get; set; }
        public float? cataloguePoint { get; set; }

        public CataloguePointDTO() { }
        public CataloguePointDTO(int? catalogueId, float? cataloguePoint)
        {
            this.catalogueId = catalogueId;
            this.cataloguePoint = cataloguePoint;
        }
    }
}
