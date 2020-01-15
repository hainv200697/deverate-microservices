using System.Collections.Generic;
using Newtonsoft.Json;

namespace TestManagementServices.Model
{
    public class CatalogueStatistic
    {
        [JsonIgnore]
        public int CatalogueInConfigId { get; set; }
        public string catalogueName { get; set; }
        public List<CatalogueSeriesItem> series { get; set; }
    }
}
