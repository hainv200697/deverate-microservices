using System;
using Newtonsoft.Json;

namespace TestManagementServices.Model
{
    public class CatalogueSeriesItem
    {
        public string date { get; set; }
        public double point { get; set; }
        [JsonIgnore]
        public int numberTest { get; set; }
    }
}