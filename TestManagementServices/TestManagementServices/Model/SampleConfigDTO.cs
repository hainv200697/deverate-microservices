using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    [JsonObject("SampleConfigDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SampleConfigDTO
    {
        [JsonProperty("catalogueInConfigurations")]
        public List<CatalogueInSampleTestDTO> catalogueInSamples { get; set; }
        public SampleConfigDTO() { }
        public SampleConfigDTO(List<CatalogueInSampleTestDTO> catalogueInSamples)
        {
            this.catalogueInSamples = catalogueInSamples;

        }

    }

    public class CatalogueInSampleTestDTO
    {
        public int companyCatalogueId { get; set; }
        public int numberQuestion { get; set; }
        public CatalogueInSampleTestDTO() { }
        public CatalogueInSampleTestDTO(int companyCatalogueId, int numberQuestion)
        {
            this.companyCatalogueId = companyCatalogueId;
            this.numberQuestion = numberQuestion;
        }
    }
}
