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
        public int? totalQuestion { get; set; }
        public int? duration { get; set; }
        public int companyId { get; set; }
        [JsonProperty("catalogueInConfigurations")]
        public List<CatalogueInSampleTestDTO> catalogueInSamples { get; set; }


        public SampleConfigDTO() { }
        public SampleConfigDTO(int? totalQuestion, int? duration, List<CatalogueInSampleTestDTO> catalogueInSamples)
        {
            this.totalQuestion = totalQuestion;
            this.duration = duration;
            this.catalogueInSamples = catalogueInSamples;

        }

    }

    public class CatalogueInSampleTestDTO
    {
        [JsonProperty("catalogueId")]
        public int? cicId { get; set; }
        public double? weightPoint { get; set; }
        public CatalogueInSampleTestDTO() { }
        public CatalogueInSampleTestDTO(int? cicId, double? weightPoint)
        {
            this.cicId = cicId;
            this.weightPoint = weightPoint;
        }
    }
}
