using System;
using Newtonsoft.Json;

namespace TestManagementServices.Model
{
    public class CatalogueSeriesItem
    {
        public string name { get; set; }
        public double value { get; set; }
        [JsonIgnore]
        public int numberTest { get; set; }
    }
}